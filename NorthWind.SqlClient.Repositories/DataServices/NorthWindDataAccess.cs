using NorthWind.Sales.BusinessObjects.POCOEntities;
using System.Data;
using System.Data.SqlClient;

namespace NorthWind.SqlClient.Repositories.DataServices
{
    public class NorthWindDataAccess
    {
        readonly string ConnectionString;

        public NorthWindDataAccess(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<IEnumerable<Order>> GetAllOrders(bool withID)
        {

            List<Order> result = new List<Order>();

            try
            {
                using SqlCommand cmd = new SqlCommand();
                using DataSet dataSet = new DataSet();

                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandText = "[GetAllOrders]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@WithID", withID);

                using (SqlDataAdapter SqlData = new SqlDataAdapter(cmd))
                {
                    SqlData.Fill(dataSet);
                }


                if (dataSet?.Tables?.Count > 0 && dataSet.Tables[0]?.Rows?.Count > 0)
                {
                    foreach (DataRow data in dataSet.Tables[0].Rows)
                    {
                        result.Add(new Order
                        {
                            Id = int.Parse(data["Id"].ToString()),
                            CustomerId = data["CustomerId"].ToString(),
                            ShipAddress = data["ShipAddress"].ToString(),
                            OrderDate = DateTime.Parse(data["OrderDate"].ToString()),
                            Discount = double.Parse(data["Discount"].ToString()),
                            ShipCountry = data["ShipCountry"].ToString(),
                            ShipCity = data["ShipCity"].ToString(),
                            ShipPostalCode = data["ShipPostalCode"].ToString(),

                        });
                    }
                }
                else
                {
                    //error = new
                    //{
                    //    Resultado = "0",
                    //    Descripcion = "No se encontró ningún registro"
                    //};
                }

            }
            catch (Exception ex)
            {
                //error = new
                //{
                //    Descripcion = "Error al intentar recuperar los datos, " + ex.Message,
                //    Resultado = "0"
                //};
            }

            return result;
        }


    }
}
