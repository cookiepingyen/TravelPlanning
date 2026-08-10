using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Extension
{
    public static class CollectionExtensions
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            // 1. Fetch properties of the generic type
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            table.TableName = "Items";
            // 2. Define DataTable columns based on properties
            foreach (PropertyDescriptor prop in properties)
            {
                // Safely extract the underlying type if it is Nullable
                Type columnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, columnType);
            }

            // 3. Populate DataTable rows
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    // Substitute null values with DBNull.Value to prevent data exceptions
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
