using Aspose.Words;
using CommunityToolkit.Mvvm.Messaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Enums;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Routes;
using IOCServiceCollection;
using Microsoft.Office.Interop.Word;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
// Aspose.Words、Word Interop、OpenXml 三邊的 Paragraph / Run / Text / Document 等型別同名，用別名區分
using O = DocumentFormat.OpenXml;
using W = DocumentFormat.OpenXml.Wordprocessing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Models.DAO;
using TravelPlanning.Extension;
using TravelPlanning.Models.DTO;
using TravelPlanning.Presenters;
using TravelPlanning.Utilities;
using TravelPlanning.Views.Pages;
using static TravelPlanning.Contracts.MyTripContract;
using static TravelPlanning.Contracts.TripDetailContract;
using Application = Microsoft.Office.Interop.Word.Application;
using Document = Aspose.Words.Document;
using Task = System.Threading.Tasks.Task;


namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class TripDetailContext : ITripDetailView
    {
        [DoNotNotify]
        public Func<int, Task<byte[]>> CaptureMapImage { get; set; }

        public TripDTO CurrentTrip { get; set; }
        public Guid TripID { get; set; }
        public TripDaysContext CurrentDay { get; set; }

        public ObservableCollection<TripDaysContext> tripDaysContexts { get; set; }

        #region commands
        public ICommand AddDayBtnCommand { get; set; }
        public ICommand DeleteDayBtnCommand { get; set; }
        public ICommand SelectDayCommand { get; set; }
        public ICommand OpenAddPlaceCommand { get; set; }
        public ICommand CancelAddPlaceCommand { get; set; }
        public ICommand ConfirmAddPlaceCommand { get; set; }
        public ICommand SelectAddPlaceCommand { get; set; }
        public ICommand TogglePlaceMenuCommand { get; set; }
        public ICommand DeletePlaceCommand { get; set; }
        public ICommand EditPlaceCommand { get; set; }
        public ICommand CancelEditPlaceCommand { get; set; }
        public ICommand ConfirmEditPlaceCommand { get; set; }
        public ICommand ChangeTravelModeCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public bool IsAddPlacePopupOpen { get; set; }
        #endregion

        public PlaceDetailResModel PendingPlace { get; set; }

        public string PendingTimeText { get; set; }

        public string PendingTransitTimeText { get; set; }

        public bool IsCustom { get; set; }

        public string PendingCustomHourText { get; set; }

        public string PendingCustomMinuteText { get; set; }

        public bool IsEditPlacePopupOpen { get; set; }

        public TripDayPlaceContext EditingPlace { get; set; }

        public string EditPendingStayTimeText { get; set; }

        public string EditPendingTransitTimeText { get; set; }

        public bool EditIsCustom { get; set; }

        public string EditPendingCustomHourText { get; set; }

        public string EditPendingCustomMinuteText { get; set; }

        public ITripDetailPresenter tripDetailPresenter { get; set; }


        public IGoogleAPIContext GoogleAPIContext { get; set; }

        public int MapVersion { get; set; }

        public TravelMode TravelMode { get; set; } = TravelMode.DRIVE;


        public TripDetailContext(TripDTO tripDTO, PresenterFactory presenterFactory, IGoogleAPIContext googleAPIContext)
        {
            CurrentTrip = tripDTO;
            TripID = tripDTO.Id;

            GoogleAPIContext = googleAPIContext;

            this.tripDetailPresenter = presenterFactory.Create<ITripDetailPresenter, ITripDetailView>(this);

            tripDetailPresenter.GetTripRequest(TripID);



            this.AddDayBtnCommand = new RelayCommand<Guid>(guid =>
            {
                tripDetailPresenter.CreateTripDay(guid);
            });

            this.DeleteDayBtnCommand = new RelayCommand<TripDaysContext>(tripDay =>
            {
                tripDetailPresenter.DeleteTripDay(tripDay.Id);
                tripDaysContexts.Remove(tripDay);
                var firstDay = tripDaysContexts.OrderBy(x => x.Day).First();
                firstDay.IsChecked = true;

                for (int i = 0; i < tripDaysContexts.Count; i++)
                {
                    TripDaysContext x = tripDaysContexts[i];
                    x.Day = i + 1;
                    x.Date = firstDay.Date.AddDays(i);
                    x.IsChecked = false;
                }

            });

            this.DeletePlaceCommand = new RelayCommand<TripDayPlaceContext>(async (tripDayPlace) =>
            {
                tripDetailPresenter.DeleteTripDayPlace(tripDayPlace.Id);
                CurrentDay.TripDayPlaces.Remove(tripDayPlace);
                await RefreshMapAsync();
            });



            this.SelectDayCommand = new RelayCommand<TripDaysContext>(async tripDay =>
            {
                CurrentDay = tripDay;
                await LoadCurrentDayMapAsync();
            });

            this.SelectAddPlaceCommand = new RelayCommand<PlaceDetailResModel>(place =>
            {
                PendingPlace = place;
            });

            this.TogglePlaceMenuCommand = new RelayCommand<TripDayPlaceContext>(place =>
            {
                if (CurrentDay?.TripDayPlaces == null) return;

                bool isOpening = !place.IsMenuOpen;

                foreach (TripDayPlaceContext item in CurrentDay.TripDayPlaces)
                {
                    item.IsMenuOpen = false;
                }

                place.IsMenuOpen = isOpening;
            });

            this.EditPlaceCommand = new RelayCommand<TripDayPlaceContext>(place =>
            {
                place.IsMenuOpen = false;

                EditingPlace = place;
                EditPendingStayTimeText = place.Stay_time.ToString();
                EditPendingTransitTimeText = place.Transit_time.ToString();
                EditIsCustom = place.Is_custom;
                EditPendingCustomHourText = place.Is_custom ? place.Start_time.Hour.ToString("00") : string.Empty;
                EditPendingCustomMinuteText = place.Is_custom ? place.Start_time.Minute.ToString("00") : string.Empty;
                IsEditPlacePopupOpen = true;
            });

            this.CancelEditPlaceCommand = new RelayCommand(() =>
            {
                EditingPlace = null;
                EditPendingStayTimeText = string.Empty;
                EditPendingTransitTimeText = string.Empty;
                EditIsCustom = false;
                EditPendingCustomHourText = string.Empty;
                EditPendingCustomMinuteText = string.Empty;
                IsEditPlacePopupOpen = false;
            });

            this.ConfirmEditPlaceCommand = new RelayCommand<TripDayPlaceContext>(place =>
            {
                place.Is_custom = EditIsCustom;
                place.Stay_time = int.Parse(EditPendingStayTimeText);
                place.Transit_time = int.Parse(EditPendingTransitTimeText);
                if (EditIsCustom)
                {
                    DateTime customTime = new DateTime(place.Start_time.Year, place.Start_time.Month, place.Start_time.Day, int.Parse(EditPendingCustomHourText), int.Parse(EditPendingCustomMinuteText), 0);
                    place.Start_time = customTime;
                }

                TripDayPlaceDAO tripDayPlaceDAO = Mapper.Map<TripDayPlaceContext, TripDayPlaceDAO>(place);
                tripDetailPresenter.UpdateTripDayPlace(tripDayPlaceDAO);

                IsEditPlacePopupOpen = false;
            });

            this.ChangeTravelModeCommand = new RelayCommand<string>(async mode =>
            {
                TravelMode = (TravelMode)Enum.Parse(typeof(TravelMode), mode);

                await LoadCurrentDayMapAsync();
            });

            this.OpenAddPlaceCommand = new RelayCommand(() =>
            {
                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = true;
            });

            this.CancelAddPlaceCommand = new RelayCommand(() =>
            {
                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = false;
            });

            this.ConfirmAddPlaceCommand = new RelayCommand(() =>
            {
                if (PendingPlace == null) return;

                int stayTime = int.TryParse(PendingTimeText, out int parsedStayTime) ? parsedStayTime : 30;
                int transitTime = int.TryParse(PendingTransitTimeText, out int parsedTransitTime) ? parsedTransitTime : 0;

                bool hasCustomTime = IsCustom
                    && int.TryParse(PendingCustomHourText, out int customHour) && customHour >= 0 && customHour <= 23
                    && int.TryParse(PendingCustomMinuteText, out int customMinute) && customMinute >= 0 && customMinute <= 59;

                DateTime startTime;
                if (hasCustomTime)
                {
                    startTime = new DateTime(CurrentDay.Date.Year, CurrentDay.Date.Month, CurrentDay.Date.Day, int.Parse(PendingCustomHourText), int.Parse(PendingCustomMinuteText), 0);
                }
                else if (CurrentDay.TripDayPlaces == null || CurrentDay.TripDayPlaces.Count == 0)
                {
                    startTime = CurrentDay.StartTime;
                }
                else
                {
                    TripDayPlaceContext lastPlace = CurrentDay.TripDayPlaces.OrderBy(x => x.Start_time).Last();
                    startTime = lastPlace.Start_time.AddMinutes(lastPlace.Transit_time + lastPlace.Stay_time);
                }

                TripDayPlaceDAO tripDayPlace = new TripDayPlaceDAO()
                {
                    TripDays_id = CurrentDay.Id,
                    Place_id = PendingPlace.result.place_id,
                    Place_name = PendingPlace.result.name,
                    Start_time = startTime,
                    Transit_time = transitTime,
                    Stay_time = stayTime,
                    Is_custom = IsCustom,
                };

                tripDetailPresenter.AddTripDayPlace(tripDayPlace);

                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = false;
            });

            this.ExportCommand = new RelayCommand(() =>
            {
                ExportDataToWord();
            });
        }

        public async Task<PlaceDetailResModel> GetPlaceDetail(string selectedItem, bool with_all_field)
        {
            PlaceDetailRequest placeDetailRequest = new PlaceDetailRequest();
            placeDetailRequest.placeId = selectedItem;

            if (!with_all_field)
            {
                placeDetailRequest.fields = new PlaceDetailInputFields[] { PlaceDetailInputFields.name, PlaceDetailInputFields.formatted_address, PlaceDetailInputFields.type };
            }

            PlaceDetailResModel placeDetailResModel = await GoogleAPIContext.Place.PlaceDetail(placeDetailRequest);

            return placeDetailResModel;
        }

        public async void OnTripsResponse(List<TripDaysDAO> tripDays)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TripDaysDAO, TripDaysContext>();
                cfg.CreateMap<TripDayPlaceDAO, TripDayPlaceContext>();
            });
            var mapper = config.CreateMapper();

            List<TripDaysContext> days = mapper.Map<List<TripDaysContext>>(tripDays);

            foreach (TripDaysContext day in days)
            {
                if (day.TripDayPlaces != null)
                {
                    day.TripDayPlaces = new ObservableCollection<TripDayPlaceContext>(day.TripDayPlaces.OrderBy(x => x.Start_time));
                }
            }

            tripDaysContexts = new ObservableCollection<TripDaysContext>(days);
            CurrentDay = tripDaysContexts[1];

            await LoadCurrentDayMapAsync();
        }

        public async Task LoadCurrentDayMapAsync()
        {
            if (CurrentDay.TripDayPlaces.Count >= 2)
            {
                string StartPlaceId = CurrentDay.TripDayPlaces.First().Place_id;
                string EndPlaceId = CurrentDay.TripDayPlaces.Last().Place_id;

                RoutesRequest routesRequest = new RoutesRequest(StartPlaceId, EndPlaceId, mode: TravelMode, addressType: AddressType.PlaceId);
                if (CurrentDay.TripDayPlaces.Count > 2)
                {
                    List<string> intermediates = new List<string>();
                    for (int i = 1; i < CurrentDay.TripDayPlaces.Count - 1; i++)
                    {
                        intermediates.Add(CurrentDay.TripDayPlaces[i].Place_id);
                    }
                    routesRequest.intermediates = intermediates;
                }
                RoutesResModel routesResModel = await GoogleAPIContext.Route.GetRoutes(routesRequest);

                WeakReferenceMessenger.Default.Send(routesResModel.routes[0].polyline.encodedPolyline.ToList());
            }

            var places = await Task.WhenAll(CurrentDay.TripDayPlaces.Select(x => GetPlaceDetail(x.Place_id, true)));
            foreach (var place in places)
            {
                WeakReferenceMessenger.Default.Send(place);
            }
        }

        public async Task RefreshMapAsync()
        {
            MapVersion++;
            await LoadCurrentDayMapAsync();
        }

        public void OnCreateTripDaysResponse(TripDaysDAO tripDays)
        {
            TripDaysContext tripDay = Utilities.Mapper.Map<TripDaysDAO, TripDaysContext>(tripDays);
            tripDaysContexts.Add(tripDay);
        }

        public async void OnCreateTripDayPlaceResponse(TripDayPlaceDAO tripDayPlace)
        {
            TripDayPlaceContext tripDayPlaceContext = Utilities.Mapper.Map<TripDayPlaceDAO, TripDayPlaceContext>(tripDayPlace);

            if (CurrentDay.TripDayPlaces == null)
            {
                CurrentDay.TripDayPlaces = new ObservableCollection<TripDayPlaceContext>();
            }

            CurrentDay.TripDayPlaces.Add(tripDayPlaceContext);
            await RefreshMapAsync();
        }


        public void OnUpdateTripDayPlaceResponse(List<TripDayPlaceDAO> tripDayPlaceDAOs)
        {

            List<TripDayPlaceContext> tripDayPlaceContexts = Utilities.Mapper.Map<TripDayPlaceDAO, TripDayPlaceContext>(tripDayPlaceDAOs).ToList();

            CurrentDay.TripDayPlaces = new ObservableCollection<TripDayPlaceContext>(tripDayPlaceContexts);

        }



        public async Task ExportDataToWord()
        {
            Document doc = new Document("C:\\Users\\user\\source\\repos\\C#基礎專案\\TravelPlanning\\TravelPlanning\\Templates\\旅遊行程套版_v3.docx");

            // Trip 資料
            string TripName = CurrentTrip.Name;
            string TotalDays = tripDaysContexts.Count().ToString();
            string StartDate = tripDaysContexts.First().DateText;
            string EndDate = tripDaysContexts.Last().DateText;
            byte[] imgData = ConvertBitmapImageToByteArray(CurrentTrip.Cover);


            doc.MailMerge.Execute(
                new[] { "TripName", "TotalDays", "StartDate", "EndDate" },
                new object[] { $"{TripName}", $"{TotalDays}", $"{StartDate}", $"{EndDate}" }
            );


            //tripDaysContexts.ToList().ForEach(async (x) =>
            //{
            //    CurrentDay = x;
            //    await LoadCurrentDayMapAsync();
            //    x.GmapImage = CaptureMapImage?.Invoke();
            //});

            for (int i = 0; i < tripDaysContexts.Count; i++)
            {
                CurrentDay = tripDaysContexts[i];
                await LoadCurrentDayMapAsync();

                if (CaptureMapImage != null)
                {
                    CurrentDay.GmapImage = await CaptureMapImage.Invoke(390);
                }
            }

            // TripDay 資料

            doc.MailMerge.ExecuteWithRegions(new DayDataSource(tripDaysContexts));



            //DataTable productsTable = products.ToDataTable();
            //TripDaysContext Day = CurrentDay;

            //doc.MailMerge.Execute(
            //    new[] { "DayIndex", "DayDate", "DayWeekday", "DepartureTime" },

            //    new object[] { $"{1}", $"{Day.DateText}", $"{Day.Date.DayOfWeek}", $"{Day.StartTimeText}" }
            //);


            // 封面圖
            DocumentBuilder builder = new DocumentBuilder(doc);
            bool moved = builder.MoveToBookmark("TripCover");
            builder.InsertImage(imgData);

            // COM 轉檔一定要絕對路徑：相對路徑會被 Word 解讀成它自己的工作目錄
            string docxPath = Path.GetFullPath("output1.docx");
            string pdfPath = Path.GetFullPath("output1.pdf");

            doc.Save(docxPath);

            // 先清掉頁首/浮水印與頁尾的評估訊息，PDF 才能從乾淨的 docx 產生
            RemoveAllHeaders(docxPath);
            RemoveTextEverywhere(docxPath, AsposeEvaluationNotices);
            ConvertDocxToPdf(docxPath, pdfPath);




            MessageBox.Show("檔案匯出完成!");








        }

        private byte[] ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // 依你的圖片格式選擇對應的 Encoder，PNG 可保留透明度
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }


        void RemoveAllHeaders(string path)
        {
            using (var doc = WordprocessingDocument.Open(path, true))
            {
                var main = doc.MainDocumentPart;

                // 1) 移除每個節的頁首參照（default / first / even 三種都會被掃到）
                foreach (var sectPr in main.Document.Descendants<SectionProperties>())
                {
                    foreach (var href in sectPr.Elements<HeaderReference>().ToList())
                        href.Remove();

                    // 「第一頁不同」旗標
                    foreach (var tp in sectPr.Elements<TitlePage>().ToList())
                        tp.Remove();
                }

                // 2) 刪掉 header part 本體（一定要 ToList，邊列舉邊刪會炸）
                main.DeleteParts(main.HeaderParts.ToList());

                // 3) 關掉「奇偶頁不同」
                var settingsPart = main.DocumentSettingsPart;
                if (settingsPart != null)
                {
                    foreach (var e in settingsPart.Settings.Elements<EvenAndOddHeaders>().ToList())
                        e.Remove();
                    settingsPart.Settings.Save();
                }

                main.Document.Save();
            }
        }


        /// <summary>
        /// Aspose.Words 未授權時塞進文件的評估訊息。
        /// 用 Regex 而非固定字串，因為 Copyright 年份每年都會變。
        /// </summary>
        private static readonly Regex[] AsposeEvaluationNotices =
        {
            // 每一頁的頁尾（同一段裡還有頁碼欄位，所以只能刪這句、不能刪整個頁尾）
            new Regex(@"Evaluation Only\.\s*Created with Aspose\.Words\.\s*Copyright\s*\d{4}-\d{4}\s*Aspose Pty Ltd\.\s*"),
            // 本文第一段
            new Regex(@"Created with an evaluation copy of Aspose\.Words\..*?Free Temporary License\s*", RegexOptions.Singleline),
            new Regex(@"https://products\.aspose\.com/words/temporary-license/?"),
        };

        /// <summary>
        /// 從整份 docx（本文 / 頁首 / 頁尾 / 註腳）刪除符合 pattern 的文字。
        /// docx 沒有「頁」的結構，真正每頁都出現的文字必然在頁首或頁尾，所以這裡一併掃。
        /// </summary>
        private static void RemoveTextEverywhere(string docxPath, params Regex[] patterns)
        {
            if (patterns == null || patterns.Length == 0) return;

            using (var wordDoc = WordprocessingDocument.Open(docxPath, true))
            {
                var main = wordDoc.MainDocumentPart;

                var roots = new List<O.OpenXmlPartRootElement> { main.Document };
                foreach (var hp in main.HeaderParts) roots.Add(hp.Header);
                foreach (var fp in main.FooterParts) roots.Add(fp.Footer);
                if (main.FootnotesPart != null) roots.Add(main.FootnotesPart.Footnotes);
                if (main.EndnotesPart != null) roots.Add(main.EndnotesPart.Endnotes);

                foreach (var root in roots)
                {
                    // ToList()：迴圈裡會刪節點，不能邊列舉邊改
                    foreach (var para in root.Descendants<W.Paragraph>().ToList())
                        foreach (var pattern in patterns)
                            StripTextFromParagraph(para, pattern);

                    root.Save();
                }
            }
        }

        /// <summary>刪除單一段落內符合 pattern 的文字，必要時連空掉的段落一起收掉。</summary>
        private static void StripTextFromParagraph(W.Paragraph para, Regex pattern)
        {
            var texts = para.Descendants<W.Text>().ToList();
            if (texts.Count == 0) return;

            // 同一句話常被 Word 切散在多個 <w:r><w:t> 裡（rsid、拼字檢查、格式變化都會切），
            // 所以先把整段拼成一個字串並記住每個 w:t 的起始位移，比對命中後再換算回去刪。
            var sb = new StringBuilder();
            var offsets = new int[texts.Count];
            for (int i = 0; i < texts.Count; i++)
            {
                offsets[i] = sb.Length;
                sb.Append(texts[i].Text);
            }

            var matches = pattern.Matches(sb.ToString());
            if (matches.Count == 0) return;

            // 由後往前刪：先刪前面的會讓後面所有位移失效
            for (int m = matches.Count - 1; m >= 0; m--)
            {
                int hitStart = matches[m].Index;
                int hitEnd = hitStart + matches[m].Length;

                for (int i = 0; i < texts.Count; i++)
                {
                    int tStart = offsets[i];
                    int tEnd = tStart + texts[i].Text.Length;
                    if (tEnd <= hitStart || tStart >= hitEnd) continue;   // 與命中範圍無交集

                    int from = Math.Max(hitStart, tStart) - tStart;
                    int to = Math.Min(hitEnd, tEnd) - tStart;
                    texts[i].Text = texts[i].Text.Remove(from, to - from);
                }
            }

            // 清掉空殼，並保住剩餘文字的首尾空白
            foreach (var t in texts)
            {
                if (t.Text.Length == 0)
                {
                    var run = t.Parent as W.Run;
                    t.Remove();
                    // run 裡只剩格式設定 (w:rPr) 就整個移除
                    if (run != null && !run.Elements().Any(e => !(e is W.RunProperties)))
                        run.Remove();
                }
                else if (t.Text != t.Text.Trim())
                {
                    t.Space = O.SpaceProcessingModeValues.Preserve;
                }
            }

            foreach (var link in para.Descendants<W.Hyperlink>().ToList())
                if (!link.Descendants<W.Text>().Any())
                    link.Remove();

            // 整段空了才刪，但要避開兩個地雷：
            //   1) 段落裡還有圖片 / 圖形 → 不能刪
            //   2) 表格儲存格必須至少保留一個段落，否則 Word 會判定檔案損毀
            if (para.Descendants<W.Text>().Any(t => t.Text.Length > 0)) return;
            if (para.Descendants<W.Drawing>().Any() || para.Descendants<W.Picture>().Any()) return;

            var cell = para.Parent as W.TableCell;
            if (cell != null && cell.Elements<W.Paragraph>().Count() <= 1) return;

            para.Remove();
        }


        // Word COM 的 enum 值，用晚期繫結所以自己列出來，不必參考 PIA
        private const int WdExportFormatPdf = 17;   // WdExportFormat.wdExportFormatPDF

        /// <summary>
        /// 用本機安裝的 Word 把 docx 轉成 PDF。
        /// 不走 Aspose，因為未授權的 Aspose 每次 Save 都會重新加上評估標記。
        /// </summary>
        private void ConvertDocxToPdf(string docxPath, string pdfPath)
        {

            // 概念性程式碼
            Application appWord = new Application();

            try
            {
                // 背景執行，不顯示 UI
                appWord.Visible = false;

                var wordDocument = appWord.Documents.Open(Path.GetFullPath(docxPath));

                // 匯出為 PDF
                wordDocument.ExportAsFixedFormat(
                    Path.GetFullPath(pdfPath),
                    WdExportFormat.wdExportFormatPDF
                );

                wordDocument.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                appWord.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(appWord);
            }
        }
    }
}
