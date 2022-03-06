using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ASPNetForAndroid092021
{
    public class SQLGetHistoryProduct
    {
        

        public DataTable GetHistory(string s_Id)
        {
            SqlConnection sQLconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
            DataTable data = new DataTable("ARTYKUL");


            



                    string s_Operation = "SELECT A.NUMER, A.KOREKTA, convert(datetime, A.DATA - 36163) as DATA, A.KONTRAHENT, C.NUMER, convert(datetime, C.DATA_SPRZEDAZY - 36163, 103) AS DATA_SPRZEDAZY, " +
                "A.ILOSC, " +
                "A.RODZAJ, A.POZOSTALO, A.CENA, A.ZYSK " +
                    "FROM  { oj HISTORIA_MAGAZYNU A LEFT OUTER JOIN DOKUMENT_HANDLOWY_V C ON A.ID_DOK_HANDLOWEGO = C.ID_DOKUMENTU_HANDLOWEGO } , " +
                    "POZYCJA_DOKUMENTU_MAGAZYNOWEGO B " +
                    "WHERE(A.ID_ARTYKULU = @s3)  AND A.ID_POZYCJI = B.ID_POZ_DOK_MAG " +
                    "ORDER BY  A.DATA DESC, A.ID_POZYCJI DESC ";
              

            try
            {
                sQLconnection.Open();
                SqlCommand cmd1 = new SqlCommand(s_Operation, sQLconnection);


                cmd1.Parameters.AddWithValue("@s3", s_Id);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd1);

                sqlDataAdapter.Fill(data);
                cmd1.Dispose();
                sqlDataAdapter.Dispose();



                sQLconnection.Close();
                return data;
            }
            catch (Exception ex)
            {
                
                sQLconnection.Close();
                return data;
            }
        }
    }
}