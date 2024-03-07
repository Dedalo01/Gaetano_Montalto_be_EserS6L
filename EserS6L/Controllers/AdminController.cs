using EserS6L.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace EserS6L.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(Utente utente)
        {

            string connString = ConfigurationManager.ConnectionStrings["EserS6Conn"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();
                string insertQuery;
                SqlCommand insertCommand;
                if (utente.PartitaIva != null)
                {
                    insertQuery = @"INSERT INTO Utenti (Nome, Email, PartitaIva, TipoCliente)
                VALUES (@nome, @email, @partitaIva, @tipoCliente)
                 ";
                    insertCommand = new SqlCommand(insertQuery, conn);
                    insertCommand.Parameters.AddWithValue("@nome", utente.Nome);
                    insertCommand.Parameters.AddWithValue("@email", utente.Email);
                    insertCommand.Parameters.AddWithValue("@partitaIva", utente.PartitaIva);
                    insertCommand.Parameters.AddWithValue("@tipoCliente", utente.TipoCliente);
                }
                else
                {
                    insertQuery = @"INSERT INTO Utenti (Nome, Cognome, Email, CodiceFiscale, TipoCliente)
                VALUES (@nome, @cognome, @email, @codiceFiscale, @tipoCliente)
                 ";
                    insertCommand = new SqlCommand(insertQuery, conn);
                    insertCommand.Parameters.AddWithValue("@nome", utente.Nome);
                    insertCommand.Parameters.AddWithValue("@cognome", utente.Cognome);
                    insertCommand.Parameters.AddWithValue("@email", utente.Email);
                    insertCommand.Parameters.AddWithValue("@codiceFiscale", utente.CodiceFiscale);
                    insertCommand.Parameters.AddWithValue("@tipoCliente", utente.TipoCliente);

                }


                int nRows = insertCommand.ExecuteNonQuery();

                if (nRows > 0)
                {
                    return RedirectToAction("Users");

                }

            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return View();
        }

        public ActionResult Users()
        {

            string connString = ConfigurationManager.ConnectionStrings["EserS6Conn"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            List<Utente> utentiList = new List<Utente>();

            try
            {
                conn.Open();


                string selectAllusersQuery = @"SELECT * FROM Utenti";
                SqlCommand selectAllUsersCmd = new SqlCommand(selectAllusersQuery, conn);

                SqlDataReader userTableReader = selectAllUsersCmd.ExecuteReader();

                if (userTableReader.HasRows)
                {
                    while (userTableReader.Read())
                    {
                        Utente utente = new Utente();
                        utente.Id = (int)userTableReader["Id"];
                        utente.Nome = userTableReader["Nome"].ToString();
                        utente.Cognome = userTableReader["Cognome"].ToString();
                        utente.Email = userTableReader["Email"].ToString();
                        utente.CodiceFiscale = userTableReader["CodiceFiscale"].ToString();
                        utente.PartitaIva = userTableReader["PartitaIva"].ToString();
                        utente.TipoCliente = userTableReader["TipoCliente"].ToString();

                        utentiList.Add(utente);
                    }

                    return View(utentiList);
                }
                else
                {
                    return View("Error");
                }


            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return View("Error");
        }

        [HttpPost]
        public ActionResult Users(int utenteId)
        {

            return RedirectToAction("Delivery", new { userId = utenteId });
        }


        public ActionResult Delivery(int userId)
        {


            string connString = ConfigurationManager.ConnectionStrings["EserS6Conn"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                string selectUserByIdQuery = "SELECT * FROM Utenti WHERE Id = @userId";

                SqlCommand selectUserByIdCmd = new SqlCommand(selectUserByIdQuery, conn);
                selectUserByIdCmd.Parameters.AddWithValue("userId", userId);

                SqlDataReader utenteSingoloReader = selectUserByIdCmd.ExecuteReader();

                if (utenteSingoloReader.HasRows)
                {
                    utenteSingoloReader.Read();
                    Utente utente = new Utente();
                    utente.Id = userId;
                    utente.CodiceFiscale = utenteSingoloReader["CodiceFiscale"].ToString();
                    utente.PartitaIva = utenteSingoloReader["PartitaIva"].ToString();
                    utente.TipoCliente = utenteSingoloReader["TipoCliente"].ToString();

                    utenteSingoloReader.Close();

                    ViewBag.Utente = utente;

                    return View();
                }



            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return View();
        }

        [HttpPost]
        public ActionResult Delivery(Spedizione spedizione)
        {
            string connString = ConfigurationManager.ConnectionStrings["EserS6Conn"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                string createDeliveryQuery = @"INSERT INTO Spedizioni (ClienteId, DataSpedizione, Peso, CittaDestinataria, Indirizzo, NominativoDestinatario, CostoSpedizione, DataConsegnaPrevista)
                VALUES (@clienteId, @dataSpedizione, @peso, @cittaDest, @indirizzo, @nominativoDest, @costoSped, @dataConsegnaPrevista)
                ";
                SqlCommand createCmd = new SqlCommand(createDeliveryQuery, conn);
                createCmd.Parameters.AddWithValue("clienteId", spedizione.ClienteId);
                createCmd.Parameters.AddWithValue("dataSpedizione", spedizione.DataSpedizione);
                createCmd.Parameters.AddWithValue("peso", spedizione.Peso);
                createCmd.Parameters.AddWithValue("cittaDest", spedizione.CittaDestinataria);
                createCmd.Parameters.AddWithValue("indirizzo", spedizione.Indirizzo);
                createCmd.Parameters.AddWithValue("nominativoDest", spedizione.NominativoDestinatario);
                createCmd.Parameters.AddWithValue("costoSped", spedizione.CostoSpedizione);
                createCmd.Parameters.AddWithValue("dataConsegnaPrevista", spedizione.DataConsegnaPrevista);

                int nRows = createCmd.ExecuteNonQuery();
                if (nRows > 0)
                {
                    //ViewBag.IsSuccess = true;
                    TempData["IsSuccess"] = true;
                    return RedirectToAction("Delivery", new { userId = spedizione.ClienteId });
                    //return View(new { userId = spedizione.Id });
                }


            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return View();
        }

        public ActionResult ManageDeliveryStatus()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetTotalDeliveryByCity()
        {
            string connString = ConfigurationManager.ConnectionStrings["EserS6Conn"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            string selectAllDeliveryByCityQuery = "SELECT NominativoDestinatario, COUNT(*) AS TotaleSpedizioni FROM Spedizioni GROUP BY NominativoDestinatario";

            try
            {
                conn.Open();
                SqlCommand selectCmd = new SqlCommand(selectAllDeliveryByCityQuery);
                SqlDataReader reader = selectCmd.ExecuteReader();

                // Lista per memorizzare i risultati
                List<DeliveryData> results = new List<DeliveryData>();

                // Leggi i dati dal reader e aggiungili alla lista
                while (reader.Read())
                {
                    var resultItem = new DeliveryData
                    {
                        NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                        TotaleSpedizioni = Convert.ToInt32(reader["TotaleSpedizioni"])
                    };
                    results.Add(resultItem);
                }

                // Restituisci i dati in formato JSON
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log dell'errore per esaminarne i dettagli
                Console.WriteLine(ex.Message);

                // Restituisci un messaggio di errore generico
                return Json("Error");
            }
            finally
            {
                conn.Close();
            }
        }


        //public JsonResult GetTotalDeliveryByCity()
        //{
        //    string connString = ConfigurationManager.ConnectionStrings["EserS6Conn"].ToString();
        //    SqlConnection conn = new SqlConnection(connString);

        //    string selectAllDeliveryByCityQuery = "SELECT NominativoDestinatario, COUNT(*) AS TotaleSpedizioni FROM Spedizioni GROUP BY NominativoDestinatario";

        //    // Lista per memorizzare i risultati
        //    List<object> results = new List<object>();
        //    try
        //    {
        //        conn.Open();
        //        SqlCommand selectCmd = new SqlCommand(selectAllDeliveryByCityQuery);
        //        SqlDataReader reader = selectCmd.ExecuteReader();


        //        // Leggi i dati dal reader e aggiungili alla lista
        //        while (reader.Read())
        //        {
        //            var resultItem = new
        //            {
        //                NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
        //                TotaleSpedizioni = Convert.ToInt32(reader["TotaleSpedizioni"])
        //            };
        //            results.Add(resultItem);
        //        }

        //        // Restituisci i dati in formato JSON
        //        return Json(results, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Gestisci l'eccezione o restituisci un messaggio di errore
        //        return Json("Error");
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //}

    }
}