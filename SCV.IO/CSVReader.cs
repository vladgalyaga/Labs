using SCV.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SCV.IO
{
    public class CSVReader
    {
        public IEnumerable<TEntity> ReadText<TEntity>(List<string> csv, char separator = ',') where TEntity : new()
        {
            List<TEntity> result = new List<TEntity>();
            if (csv?.Any() ?? true)
                return null;

            var columnsIntText = csv.First().Split(separator).ToList();
            var propsWithColumnsInEntity = typeof(TEntity).GetProperties()
                   .Where(x => x.GetCustomAttribute<ColumnAttribure>() != null);

            Dictionary<int, PropertyColumnAtribute> columnToProperty = columnsIntText.ToDictionary(
                x => columnsIntText.IndexOf(x),
                x => new PropertyColumnAtribute
                {
                    PropertyInfo = propsWithColumnsInEntity.FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribure>().ColumnName == x),
                    ColumnAttribute = propsWithColumnsInEntity.FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribure>().ColumnName == x).GetCustomAttribute<ColumnAttribure>()
                });

            csv.RemoveAt(0);

            foreach (var line in csv)
            {
                TEntity entity = new TEntity();
                var values = line.Split(separator).ToList();
                for (int i = 0; i < values.Count; i++)
                {
                    if (!columnToProperty.ContainsKey(i))
                        continue;

                    var prop = columnToProperty[i];
                    object cellValue;
                    if (prop.ColumnAttribute.Reader == null)
                    {
                        cellValue = prop.ColumnAttribute.Reader.ReadProperty(values[i]);
                    }
                    else if (prop.PropertyInfo.PropertyType != typeof(string))
                    {
                        cellValue = Convert.ChangeType(values[i], prop.PropertyInfo.PropertyType);
                    }
                    else
                    {
                        cellValue = values[i];
                    }
                    prop.PropertyInfo.SetValue(entity, cellValue);

                }
                result.Add(entity);
            }
            return result;
        }

    }

    public class PropertyColumnAtribute
    {
        public PropertyInfo PropertyInfo { get; set; }
        public ColumnAttribure ColumnAttribute { get; set; }
    }
}