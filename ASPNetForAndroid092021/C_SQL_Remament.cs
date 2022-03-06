using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ASPNetForAndroid092021
{
    public class C_SQL_Remament
    {




            SqlConnection sQLconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);


        



        public string[] Get_Id(string s_Id)
            {
                
                byte b_Operation = 4;
                string[] tempArray = new string[4];
                DataTable data = new DataTable("ARTYKUL");
                string s_OperationImage = null;



                    s_OperationImage = "IF EXISTS ( " +
                        "SELECT ILOSC " +
                        "FROM  [dbo].[__REMAMENT_KONIEC] " +
                        "where KOD_KRESKOWY = @s3 " +
                        "UNION ALL " +
                        "ILOSC " +
                        "FROM [dbo].[__DODAJ_REMAMENT] " +
                        "WHERE KOD_KRESKOWY = @s3 " +
                        ") " +
                       "BEGIN " +
                              "SELECT top 1 INDEKS_KATALOGOWY, Id_artykulu, 'true' as 'CHECK' " +
                                "FROM  [dbo].[ARTYKUL]  " +
                                "where ID_MAGAZYNU =  @s1 and KOD_KRESKOWY = @s3 " +
                        "END " +
                         "else " +
                        "BEGIN " +
                         "SELECT top 1 INDEKS_KATALOGOWY, Id_artykulu, 'false' as 'CHECK' " +
                                "FROM  [dbo].[ARTYKUL]  " +
                                "where ID_MAGAZYNU =  @s1 and KOD_KRESKOWY = @s3 " +
                        "END ";










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
                        if (columns.Contains("ID_ARTYKULU")) tempArray[0] = (dataRow["ID_ARTYKULU"]) + "";
                        if (columns.Contains("CHECK")) tempArray[2] = (dataRow["CHECK"]) + "";


                }


                }
                catch (Exception ex)
                {
                    sQLconnection.Close();
                    tempArray[0] = "Error";
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



                    s_Operation = "IF EXISTS ( SELECT TOP 1 zdjecie from [dbo].[ARTYKUL_BLOB] WHERE INDEKS_KATALOGOWY = @INDEKS_KATALOGOWY) " +
                        "BEGIN " +
                        "SELECT TOP 1 a.ID_ARTYKULU, a.LOKALIZACJA, a.INDEKS_KATALOGOWY, a.NAZWA,  a.STAN, a.ZAREZERWOWANO, a.KOD_KRESKOWY, " +
                        " a.[POLE1], c.CENA_BRUTTO as CenaA, x.ZDJECIE, @CHECK as 'CHECK' " +
                        "FROM [dbo].[ARTYKUL]  AS a " +
                        "INNER JOIN [dbo].[CENA_ARTYKULU] AS c ON c.ID_ARTYKULU = a.ID_ARTYKULU " +
                        "INNER JOIN [dbo].[ARTYKUL_BLOB] AS x ON a.INDEKS_KATALOGOWY = x.INDEKS_KATALOGOWY " +
                        "where a.ID_MAGAZYNU =  @s1 and a.ID_Artykulu = @ID_ARTYKULU and c.ID_CENY = @CENA_A " +
                        "END " +
                        "else " +
                        "BEGIN " +
                        "SELECT TOP 1 a.ID_ARTYKULU, a.LOKALIZACJA, a.INDEKS_KATALOGOWY, a.NAZWA,  a.STAN, a.ZAREZERWOWANO, a.KOD_KRESKOWY, " +
                        " a.[POLE1], c.CENA_BRUTTO as CenaA, @CHECK as 'CHECK' " +
                        "FROM [dbo].[ARTYKUL]  AS a " +
                        "INNER JOIN [dbo].[CENA_ARTYKULU] AS c ON c.ID_ARTYKULU = a.ID_ARTYKULU " +
                        "where a.ID_MAGAZYNU =  @s1 and a.ID_Artykulu = @ID_ARTYKULU and c.ID_CENY = @CENA_A " +
                        "END";
               





                try
                {

                    if (tempArray[0] == "Error") throw new Exception();

                    sQLconnection.Open();
                    SqlCommand cmd1 = new SqlCommand(s_Operation, sQLconnection);




                    cmd1.Parameters.AddWithValue("@s1", "1");
                    cmd1.Parameters.AddWithValue("@CENA_A", "4");
                    cmd1.Parameters.AddWithValue("@CENA_W", "11");
                     cmd1.Parameters.AddWithValue("@CHECK", tempArray[2]);
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



                        if (columns.Contains("STAN")) dataRow["STAN"] = Convert.ToInt64(dataRow["STAN"]) + "";                   
                        if (columns.Contains("ZAREZERWOWANO")) dataRow["ZAREZERWOWANO"] = Convert.ToInt64(dataRow["ZAREZERWOWANO"]) + "";   
                        if (columns.Contains("CENA_BRUTTO")) dataRow["CENA_BRUTTO"] = Math.Round((decimal)dataRow["CENA_BRUTTO"], 1, MidpointRounding.AwayFromZero).ToString("0.00");
                    }
                    sQLconnection.Close();

                    return data;

                }
                catch (Exception ex)
                {
                    sQLconnection.Close();
                    string s_Operation1 = "  SELECT TOP 1 a.ID_ARTYKULU, a.LOKALIZACJA, a.INDEKS_KATALOGOWY, a.NAZWA,  a.STAN, a.ZAREZERWOWANO, a.KOD_KRESKOWY, " +
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

            public string M_Update_Location(string s_Id, string s_Stan, string s_Pracownik)
            {
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = sQLconnection;
                        command.CommandType = CommandType.Text;

                    command.CommandText = "INSERT INTO [dbo].[__DODAJ_REMAMENT](ID_ARTYKULU, a.INDEKS_KATALOGOWY, a.NAZWA, a.KOD_KRESKOWY, c.Cena_Brutto, ILOSC, PRACOWNIK ) " +
                                        " select @staffID, " +
                                        " a.INDEKS_KATALOGOWY, " +
                                        " a.NAZWA, " +
                                        " a.KOD_KRESKOWY, " +
                                        " c.Cena_Brutto, " +
                                        " @s_Stan, " +
                                        " @s_Pracownik " +
                                        " from [dbo].[ARTYKUL] as a " +
                                        " INNER JOIN [dbo].[CENA_ARTYKULU] AS c ON c.ID_ARTYKULU = a.ID_ARTYKULU " +
                                        " where a.ID_Artykulu = @staffID and a.ID_Magazynu =1 and c.ID_CENY = 4";

                   

                   
              

                    command.Parameters.AddWithValue("@staffID", s_Id);
                    command.Parameters.AddWithValue("@s_Stan", s_Stan);
                    command.Parameters.AddWithValue("@s_Pracownik", s_Pracownik);


                    sQLconnection.Open();
                    

                        int a = command.ExecuteNonQuery();

                        if (a == 1)
                            return "Dodano";
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


        public DataTable GetRemament(string s_Id)
        {
            SqlConnection sQLconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
            DataTable data = new DataTable("ARTYKUL");
            string s_Operation = "";

            if (s_Id.Contains("true"))
            {

                s_Id = s_Id.Replace("true", "");
                s_Operation = "SELECT NAZWA, INDEKS_KATALOGOWY, PRACOWNIK, ILOSC  " +
                 "FROM [dbo].[__DODAJ_REMAMENT] " +
                 "WHERE pracownik = @s3 "+
                 "UNION ALL " +
                 "NAZWA, INDEKS_KATALOGOWY, PRACOWNIK, ILOSC "+
                 "FROM [dbo].[__REMAMENT_KONIEC " +
                 "WHERE pracownik = @s3 ";
            }
            else
                s_Operation = "SELECT NAZWA, INDEKS_KATALOGOWY, PRACOWNIK, ILOSC  " +
                "FROM [dbo].[__DODAJ_REMAMENT] " +
                "WHERE ID_ARTYKULU  = @s3 " +
                "UNION ALL " +
                "NAZWA, INDEKS_KATALOGOWY, PRACOWNIK, ILOSC " +
                "FROM [dbo].[__REMAMENT_KONIEC " +
                "WHERE ID_ARTYKULU = @s3 ";








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
