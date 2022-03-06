using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ASPNetForAndroid092021
{
    public class C_SQL_Connect
    {
        SqlConnection sQLconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);




        
        

        public string[] Get_Id(string s_Id)
        {
            var isNumeric = System.Text.RegularExpressions.Regex.IsMatch(s_Id, @"^\d+$");
            byte b_Operation = 4;
            string[] tempArray = new string[3];
            DataTable data = new DataTable("ARTYKUL");
            string s_OperationImage = null ;

            if (!isNumeric & !s_Id.ToLower().Contains("wyp") & !s_Id.Contains("="))
                b_Operation = 2;
            else if (s_Id.Length > 11)
                b_Operation = 1;
            else if (s_Id.Length < 11)
                b_Operation = 0;

            if (b_Operation == 1) 
                s_OperationImage = "SELECT top 1 a.INDEKS_KATALOGOWY, a.Id_artykulu, a.Id_kategorii " +
               "FROM [dbo].[KOD_KRESKOWY]  AS d " +
               "INNER JOIN [dbo].[ARTYKUL] AS a ON d.ID_ARTYKULU = a.ID_ARTYKULU " +
               "where d.ID_MAGAZYNU =  @s1 and d.KOD_KRESKOWY = @s3 ";
            if (b_Operation == 0)
                s_OperationImage = "SELECT top 1 a.INDEKS_KATALOGOWY, a.ID_ARTYKULU, a.ID_KATEGORII " +
               "FROM [dbo].[ARTYKUL]  AS a " +
               "where a.ID_MAGAZYNU =  @s1 and a.INDEKS_KATALOGOWY = @s3 ";
            if (b_Operation == 2)
               tempArray[0] = "name";
            



            try
            {
                sQLconnection.Open();
                SqlCommand cmd1 = new SqlCommand(s_OperationImage, sQLconnection);
                cmd1.Parameters.AddWithValue("@s1", "1");
                cmd1.Parameters.AddWithValue("@s3", s_Id);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd1);

                sqlDataAdapter.Fill(data);
                cmd1.Dispose();
                sqlDataAdapter.Dispose();
                sQLconnection.Close();

                DataColumnCollection columns = data.Columns;
                foreach (DataRow dataRow in data.Select())
                {
                    if (columns.Contains("INDEKS_KATALOGOWY")) tempArray[1] = (dataRow["INDEKS_KATALOGOWY"]) + "";
                    if(columns.Contains("ID_ARTYKULU")) tempArray[0] = (dataRow["ID_ARTYKULU"]) + "";
                    if(columns.Contains("ID_KATEGORII")) tempArray[2] = (dataRow["ID_KATEGORII"]) + "";
                   
                }
                    

            }
            catch (Exception ex)
            {
                sQLconnection.Close();
                tempArray[0] =  "Error";
                return tempArray;
            }

            return tempArray;
        }

        public DataTable M_Select_Product(string s_Id)
        {

            DataTable data = new DataTable("ARTYKUL");
            string[] tablica = new string[15];
            string s_Operation = "";
            string[] tempArray = Get_Id(s_Id);






            //convert(datetime, a.DATA - 36163)


            if(tempArray[2] != "name")
            s_Operation = "IF EXISTS ( SELECT TOP 1 zdjecie from [dbo].[ARTYKUL_BLOB] WHERE INDEKS_KATALOGOWY = @INDEKS_KATALOGOWY) " +
                "BEGIN " +
                "SELECT TOP 1 a.ID_ARTYKULU, a.LOKALIZACJA, a.INDEKS_KATALOGOWY, a.NAZWA,  a.STAN, a.ZAREZERWOWANO, a.KOD_KRESKOWY, " +
                " a.[POLE1],  cA.CENA_BRUTTO as CENA_BRUTTO, cW.CENA_BRUTTO as CenaW, x.ZDJECIE " +
                "FROM [dbo].[ARTYKUL]  AS a " +
                "LEFT JOIN[dbo].[CENA_ARTYKULU] AS cA ON cA.ID_ARTYKULU = a.ID_ARTYKULU and cA.ID_CENY = @CENA_A " +
                "LEFT JOIN[dbo].[CENA_ARTYKULU] AS cW ON cW.ID_ARTYKULU = a.ID_ARTYKULU and cW.ID_CENY = @CENA_W " + 
                "INNER JOIN [dbo].[ARTYKUL_BLOB] AS x ON a.INDEKS_KATALOGOWY = x.INDEKS_KATALOGOWY " +
                "where a.ID_MAGAZYNU =  @s1 and a.ID_Artykulu = @ID_ARTYKULU " +
                "END " +
                "else " +
                "BEGIN " +
                "SELECT TOP 1 a.ID_ARTYKULU, a.LOKALIZACJA, a.INDEKS_KATALOGOWY, a.NAZWA,  a.STAN, a.ZAREZERWOWANO, a.KOD_KRESKOWY, " +
                " a.[POLE1], cA.CENA_BRUTTO as CENA_BRUTTO, cW.CENA_BRUTTO as CenaW " +
                "FROM [dbo].[ARTYKUL]  AS a " +
                "LEFT JOIN[dbo].[CENA_ARTYKULU] AS cA ON cA.ID_ARTYKULU = a.ID_ARTYKULU and cA.ID_CENY = @CENA_A " +
                "LEFT JOIN[dbo].[CENA_ARTYKULU] AS cW ON cW.ID_ARTYKULU = a.ID_ARTYKULU and cW.ID_CENY = @CENA_W " +
                "where a.ID_MAGAZYNU =  @s1 and a.ID_Artykulu = @ID_ARTYKULU " +
                "END";
             //else if(tempArray[2] == "171")

             //   s_Operation = "IF EXISTS ( SELECT TOP 1 zdjecie from [dbo].[ARTYKUL_BLOB] WHERE INDEKS_KATALOGOWY = @INDEKS_KATALOGOWY) " +
             //   "BEGIN " +
             //   "SELECT TOP 1 cA.CENA_BRUTTO as CENA_BRUTTO, cW.CENA_BRUTTO as CenaW, a.ID_ARTYKULU, a.LOKALIZACJA, a.INDEKS_KATALOGOWY, a.NAZWA,  a.STAN, a.ZAREZERWOWANO, a.KOD_KRESKOWY, " +
             //   " a.[POLE1], x.ZDJECIE " +
             //   "FROM [dbo].[ARTYKUL]  AS a " +
             //   "LEFT JOIN[dbo].[CENA_ARTYKULU] AS cA ON cA.ID_ARTYKULU = a.ID_ARTYKULU and cA.ID_CENY = @CENA_A " +
             //   "LEFT JOIN[dbo].[CENA_ARTYKULU] AS cW ON cW.ID_ARTYKULU = a.ID_ARTYKULU and cW.ID_CENY = @CENA_W " +
             //   "INNER JOIN [dbo].[ARTYKUL_BLOB] AS x ON  x.INDEKS_KATALOGOWY = a.INDEKS_KATALOGOWY " +
             //   "where a.ID_MAGAZYNU =  @s1 and a.ID_Artykulu = @ID_ARTYKULU " +
             //   "END " +
             //   "else " +
             //   "BEGIN " +
             //   "SELECT TOP 1 cA.CENA_BRUTTO as CENA_BRUTTO, cW.CENA_BRUTTO as CenaW,  a.ID_ARTYKULU, a.LOKALIZACJA, a.INDEKS_KATALOGOWY, a.NAZWA,  a.STAN, a.ZAREZERWOWANO, a.KOD_KRESKOWY, " +
             //   " a.[POLE1] " +
             //   "FROM [dbo].[ARTYKUL]  AS a " +
             //   "LEFT JOIN[dbo].[CENA_ARTYKULU] AS cA ON cA.ID_ARTYKULU = a.ID_ARTYKULU and cA.ID_CENY = @CENA_A " +
             //   "LEFT JOIN[dbo].[CENA_ARTYKULU] AS cW ON cW.ID_ARTYKULU = a.ID_ARTYKULU and cW.ID_CENY = @CENA_W " +
             //   "where a.ID_MAGAZYNU =  @s1 and a.ID_Artykulu = @ID_ARTYKULU " +
             //   "END";



                if (tempArray[0] == "name")
                s_Operation = "SELECT  INDEKS_KATALOGOWY, NAZWA,  STAN  " +
                    "FROM [dbo].[ARTYKUL] " +
                    "where ID_MAGAZYNU =  @s1 and Stan > 0 and Nazwa like '%' + @s3 + '%'";

                



                try
                {

                   if (tempArray[0] == "Error") throw new Exception();

                    sQLconnection.Open();
                    SqlCommand cmd1 = new SqlCommand(s_Operation, sQLconnection);




                    cmd1.Parameters.AddWithValue("@s1", "1");
                    cmd1.Parameters.AddWithValue("@CENA_A", "4");
                    cmd1.Parameters.AddWithValue("@CENA_W", "11");
                    cmd1.Parameters.AddWithValue("@ID_ARTYKULU", tempArray[0]);
                    cmd1.Parameters.AddWithValue("@INDEKS_KATALOGOWY", tempArray[1]);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd1);

                    sqlDataAdapter.Fill(data);
                    cmd1.Dispose();
                    sqlDataAdapter.Dispose();

                

                    DataColumnCollection columns = data.Columns;

                    if (data.Rows.Count == 0)
                    {
                 

                        throw new Exception();
                    }


                    foreach (DataRow dataRow in data.Select())
                    {



                        //tablica[0] = result[0]["ID_ARTYKULU"].ToString();
                        //tablica[1] = result[0]["NAZWA"].ToString();
                        //tablica[2] = Convert.ToInt64(result[0]["STAN"]) + "";
                        if (columns.Contains("STAN")) dataRow["STAN"] = Convert.ToInt64(dataRow["STAN"]) + "";
                        //tablica[9] = Convert.ToInt64(result[0]["ZAREZERWOWANO"]) + "";
                        if (columns.Contains("ZAREZERWOWANO")) dataRow["ZAREZERWOWANO"] = Convert.ToInt64(dataRow["ZAREZERWOWANO"]) + "";
                        //tablica[3] = result[0]["INDEKS_KATALOGOWY"].ToString();
                        //tablica[4] = result[0]["KOD_KRESKOWY"].ToString();
                        //tablica[5] = result[0]["LOKALIZACJA"].ToString();
                        //tablica[6] = result[0]["POLE1"].ToString();
                        //tablica[8] = Math.Round((decimal)result[0]["CENA_BRUTTO"], 1, MidpointRounding.AwayFromZero).ToString("0.00");
                        if (columns.Contains("CENA_BRUTTO")) dataRow["CENA_BRUTTO"] = Math.Round((decimal)dataRow["CENA_BRUTTO"], 1, MidpointRounding.AwayFromZero).ToString("0.00");
                    if (dataRow["CenaW"] != DBNull.Value)
                         dataRow["CenaW"] = Math.Round((decimal)dataRow["CenaW"], 1, MidpointRounding.AwayFromZero).ToString("0.00");
                 


                }
                    //// if (columns.Contains("PROMOCJA_OD"))
                    //   if (dataRow["PROMOCJA_OD"] != DBNull.Value)
                    
                            //tablica[11] = dataRow["PROMOCJA_OD"].ToString();
                            //tablica[12] = dataRow["PROMOCJA_DO"].ToString();
                            //tablica[13] = dataRow["CENA_PROMOCJI_B"].ToString();
                            //tablica[14] = dataRow["Column1"].ToString();

                            //if (int.Parse(dataRow["Column1"].ToString()) >= int.Parse(dataRow["PROMOCJA_OD"].ToString()) & int.Parse(dataRow["Column1"].ToString()) <= int.Parse(dataRow["PROMOCJA_DO"].ToString()))
                            //   dataRow["Column1"] = "true";
                            // else
                            // dataRow["Column1"] = "false";
                            //}




                           // if (columns.Contains("ZDJECIE"))
                            //{

                            //    if (dataRow["ZDJECIE"] != DBNull.Value)
                            //    {
                            //        //byte[] Img = (byte[])dataRow["ZDJECIE"];
                            //        //tablica[10] = Convert.ToBase64String(Img);
                            //        // dataRow["ZDJECIE"] = Convert.ToBase64String(Img);
                            //    }


                            //}

                        
                    
                        sQLconnection.Close();

                        return data;
                
            }
            catch (Exception ex)
            {
                sQLconnection.Close();
                string s_Operation1 ="  SELECT TOP 1 a.ID_ARTYKULU, a.LOKALIZACJA, a.INDEKS_KATALOGOWY, a.NAZWA,  a.STAN, a.ZAREZERWOWANO, a.KOD_KRESKOWY, " +
                    "a.PROMOCJA_OD, a.PROMOCJA_DO, a.CENA_PROMOCJI_B, a.[POLE1], c.CENA_BRUTTO, x.ZDJECIE, convert(int, getdate())+36163 " +
                    "FROM [dbo].[ARTYKUL]  AS a " +
                    "INNER JOIN [dbo].[CENA_ARTYKULU] AS c ON c.ID_ARTYKULU = a.ID_ARTYKULU " +
                    "INNER JOIN [dbo].[ARTYKUL_BLOB] AS x ON  x.INDEKS_KATALOGOWY = a.INDEKS_KATALOGOWY " +
                    "where a.ID_ARTYKULU =  110633";
               
                sQLconnection.Open();
                SqlCommand cmd1 = new SqlCommand(s_Operation1, sQLconnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd1);

                sqlDataAdapter.Fill(data);
                cmd1.Dispose();
                sqlDataAdapter.Dispose();

                DataColumnCollection columns = data.Columns;
                foreach (DataRow dataRow in data.Select())
                {
                    if (columns.Contains("STAN")) dataRow["STAN"] = Convert.ToInt64(dataRow["STAN"]) + "";
                    if (columns.Contains("ZAREZERWOWANO")) dataRow["ZAREZERWOWANO"] = Convert.ToInt64(dataRow["ZAREZERWOWANO"]) + "";
                    if (columns.Contains("CENA_BRUTTO")) dataRow["CENA_BRUTTO"] = Math.Round((decimal)dataRow["CENA_BRUTTO"], 1, MidpointRounding.AwayFromZero).ToString("0.00");

                }
                sQLconnection.Close();

                return data;
            }
        }

        public string M_Update_Location(string s_Id, string s_Location, string s_Ean)
        {
            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sQLconnection;
                    command.CommandType = CommandType.Text;
                    //if (idOrCatalog)
                    if(s_Ean != "niema")
                    command.CommandText = "UPDATE[dbo].[ARTYKUL]  SET [LOKALIZACJA] = @staffName, KOD_KRESKOWY = @KOD_KRESKOWY  where ID_MAGAZYNU = @staffMag and ID_ARTYKULU = @staffID";
                    else
                        command.CommandText = "UPDATE[dbo].[ARTYKUL]  SET [LOKALIZACJA] = @staffName  where ID_MAGAZYNU = @staffMag and ID_ARTYKULU = @staffID";
                    //command.CommandText = "UPDATE[dbo].[ARTYKUL]  SET[LOKALIZACJA] = @staffName where ID_MAGAZYNU = @staffMag and INDEKS_KATALOGOWY = @staffID";

                    command.Parameters.AddWithValue("@staffID", s_Id);
                    if (s_Location.Contains(")))"))
                    {
                        s_Location = s_Location.Replace(")))", @"/");
                    }
                    if (s_Location.Contains("%20"))
                    {
                        s_Location = s_Location.Replace("%20", @" ");
                    }
                    if (s_Location.Contains(")l)"))
                    {
                        s_Location = s_Location.Replace(")l)", @"ł");
                    }
                    if (s_Location.Contains(")L)"))
                    {
                        s_Location = s_Location.Replace(")L)", @"Ł");
                    }
                    command.Parameters.AddWithValue("@staffName", s_Location);
                    command.Parameters.AddWithValue("@staffMag", "1");
                    command.Parameters.AddWithValue("@KOD_KRESKOWY", s_Ean);

                    sQLconnection.Open();
                   

                    int a = command.ExecuteNonQuery();

                    if (a == 1)
                        return "lokalizacja zmieniona";
                    else
                        return "Błąd, dla Pawła !!!";
                }
            }
            catch (SqlException ex)
            {
                return "Błąd, dla Pawła";
            }
            finally
            {
                sQLconnection.Close();
            }
        }




    }
}