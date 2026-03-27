using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comfort24.Connection;

namespace Comfort24
{
    internal class MaterialCalculator
    {

        public static int CalculateMaterial(int productTypeId, int materialTypeId, int productQuantity, double param1, double param2)
        {
            try
            {
                var productType = DBConnection.comfort.ProductType
                    .FirstOrDefault(pt => pt.Id_prodtype == productTypeId);
                var materialType = DBConnection.comfort.TypeMaterial
                    .FirstOrDefault(mt => mt.Id_type_material == materialTypeId);
                if (productType == null || materialType == null)
                {
                    return -1;
                }
                if (productQuantity <= 0 || param1 <= 0 || param2 <= 0)
                {
                    return -1;
                }
                double coefficient = Convert.ToDouble(productType.Coefficient);
                double defectPercentage = Convert.ToDouble(materialType.LostProcent);
                double materialPerUnit = param1 * param2 * coefficient;
                double totalMaterial = materialPerUnit * productQuantity;
                double result = totalMaterial * (1 + defectPercentage / 100);
                return (int)Math.Ceiling(result);
            }
            catch
            {
                return -1;
            }
        }
    }
}
