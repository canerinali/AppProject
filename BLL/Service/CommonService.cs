using DAL.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BLL.Service
{
    public class CommonService
    {
        string connStr = ConfigurationManager.ConnectionStrings["Cnn"].ConnectionString;

        #region Login işlemleri
        public LoginDto Login(LoginDto model)
        {
            try
            {
                var param = new SqlParameter[5];
                param[0] = new SqlParameter("@Id", model.Id);
                param[1] = new SqlParameter("@UserName", model.UserName);
                param[2] = new SqlParameter("@Password", model.Password);
                param[3] = new SqlParameter("@NameSurname", model.NameSurname);
                param[4] = new SqlParameter("@OperationType", "LOGIN");

                var cmd = new SqlCommand("prdLogin", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<LoginDto>();
                resultResponse = ConvertDataTable<LoginDto>(ds.Tables[0]);
                var result = resultResponse.Count > 0 ? resultResponse.SingleOrDefault() : null;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public LoginDto InsertLogin(LoginDto model)
        {
            try
            {
                var cn = new SqlConnection(connStr);
                var cmd = new SqlCommand("prdLogin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@UserName", model.UserName);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.AddWithValue("@NameSurname", model.NameSurname);
                cmd.Parameters.AddWithValue("@OperationType", "INS");
                cn.Open();
                var result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
                if (result > 0)
                {
                    var login = Login(new LoginDto()
                      {
                          Password = model.Password,
                          UserName = model.UserName
                      });
                    return login ?? null;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteLogin(LoginDto model)
        {
            try
            {
                var param = new SqlParameter[5];
                param[0] = new SqlParameter("@Id", model.Id);
                param[1] = new SqlParameter("@UserName", model.UserName);
                param[2] = new SqlParameter("@Password", model.Password);
                param[3] = new SqlParameter("@NameSurname", model.NameSurname);
                param[4] = new SqlParameter("@OperationType", "DEL");

                var cmd = new SqlCommand("prdLogin", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<LoginDto> GetUsers()
        {
            try
            {
                var param = new SqlParameter[5];
                param[0] = new SqlParameter("@Id", 0);
                param[1] = new SqlParameter("@UserName", "");
                param[2] = new SqlParameter("@Password", "");
                param[3] = new SqlParameter("@NameSurname", "");
                param[4] = new SqlParameter("@OperationType", "SELECT");

                var cmd = new SqlCommand("prdLogin", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<LoginDto>();
                resultResponse = ConvertDataTable<LoginDto>(ds.Tables[0]);
                return resultResponse.Count > 0 ? resultResponse : new List<LoginDto>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region City işlemleri
        public int InsertCity(CityDto model)
        {
            try
            {
                var operationType = model.Id > 0 ? "UPD" : "INS";
                var cn = new SqlConnection(connStr);
                var cmd = new SqlCommand("prdCityOperation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@OperationType", operationType);
                cn.Open();
                var result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool DeleteCity(int id)
        {
            try
            {
                var cn = new SqlConnection(connStr);
                var cmd = new SqlCommand("prdCityOperation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", 0);
                cmd.Parameters.AddWithValue("@OperationType", "DEL");
                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<CityDto> GetCity()
        {
            try
            {
                var param = new SqlParameter[3];
                param[0] = new SqlParameter("@Id", 0);
                param[1] = new SqlParameter("@Name", "");
                param[2] = new SqlParameter("@OperationType", "SELECT");

                var cmd = new SqlCommand("prdCityOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<CityDto>();
                resultResponse = ConvertDataTable<CityDto>(ds.Tables[0]);
                return resultResponse.Count > 0 ? resultResponse : new List<CityDto>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CityDto GetByCityId(int id)
        {
            try
            {
                var param = new SqlParameter[3];
                param[0] = new SqlParameter("@Id", id);
                param[1] = new SqlParameter("@Name", "");
                param[2] = new SqlParameter("@OperationType", "GETBYID");

                var cmd = new SqlCommand("prdCityOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<CityDto>();
                resultResponse = ConvertDataTable<CityDto>(ds.Tables[0]);
                var result = resultResponse.Count > 0 ? resultResponse.SingleOrDefault() : null;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region City fiyat işlemleri
        public int InsertCityAndPrice(CityAndPriceDto model)
        {
            try
            {
                var operationType = model.Id > 0 ? "UPD" : "INS";
             
                var cn = new SqlConnection(connStr);
                var cmd = new SqlCommand("prdCityForPriceOperation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@ToCityId", model.ToCityId);
                cmd.Parameters.AddWithValue("@FromCityId", model.FromCityId);
                cmd.Parameters.AddWithValue("@Price", model.Price);
                cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                cmd.Parameters.AddWithValue("@OperationType", operationType);
                cn.Open();
                var result = (int)cmd.ExecuteNonQuery();

                cn.Close();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool DeleteCityAndPric(int id)
        {
            try
            {
                var cn = new SqlConnection(connStr);
                var cmd = new SqlCommand("prdCityForPriceOperation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@ToCityId", 0);
                cmd.Parameters.AddWithValue("@FromCityId", 0);
                cmd.Parameters.AddWithValue("@Price", 0);
                cmd.Parameters.AddWithValue("@CompanyId", 0);
                cmd.Parameters.AddWithValue("@OperationType", "DEL");
                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();
                //var param = new SqlParameter[5];
                //param[0] = new SqlParameter("@Id", id);
                //param[1] = new SqlParameter("@ToCityId", 0);
                //param[2] = new SqlParameter("@FromCityId", 0);
                //param[3] = new SqlParameter("@Price", 0);
                //param[4] = new SqlParameter("@OperationType", "DEL");

                //var cmd = new SqlCommand("prdCityForPriceOperation", new SqlConnection(connStr));
                //cmd.CommandType = CommandType.StoredProcedure;

                //for (int i = 0; i < param.Length; i++)
                //{
                //    cmd.Parameters.Add(param[i]);
                //}

                //cmd.CommandTimeout = (1000 * 60 * 10);

                //cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<CityAndPriceDto> GetCityAndPric()
        {
            try
            {
                var param = new SqlParameter[6];
                param[0] = new SqlParameter("@Id", 0);
                param[1] = new SqlParameter("@ToCityId", 0);
                param[2] = new SqlParameter("@FromCityId", 0);
                param[3] = new SqlParameter("@Price", 0);
                param[4] = new SqlParameter("@CompanyId", 0);
                param[5] = new SqlParameter("@OperationType", "SELECT");

                var cmd = new SqlCommand("prdCityForPriceOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<CityAndPriceDto>();
                resultResponse = ConvertDataTable<CityAndPriceDto>(ds.Tables[0]);
                return resultResponse.Count > 0 ? resultResponse : new List<CityAndPriceDto>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CityAndPriceDto> GetCityForCompanyId(int companyId)
        {
            try
            {
                var param = new SqlParameter[6];
                param[0] = new SqlParameter("@Id", 0);
                param[1] = new SqlParameter("@ToCityId", 0);
                param[2] = new SqlParameter("@FromCityId", 0);
                param[3] = new SqlParameter("@Price", 0);
                param[4] = new SqlParameter("@CompanyId", companyId);
                param[5] = new SqlParameter("@OperationType", "GETCOMPANYFORCİTY");

                var cmd = new SqlCommand("prdCityForPriceOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<CityAndPriceDto>();
                resultResponse = ConvertDataTable<CityAndPriceDto>(ds.Tables[0]);
                return resultResponse.Count > 0 ? resultResponse : new List<CityAndPriceDto>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CityAndPriceDto GetPrice(int toCityId, int fromCityId)
        {
            try
            {
                var param = new SqlParameter[6];
                param[0] = new SqlParameter("@Id", 0);
                param[1] = new SqlParameter("@ToCityId", toCityId);
                param[2] = new SqlParameter("@FromCityId", fromCityId);
                param[3] = new SqlParameter("@Price", 0);
                param[4] = new SqlParameter("@CompanyId", 0);
                param[5] = new SqlParameter("@OperationType", "GETPRICE");

                var cmd = new SqlCommand("prdCityForPriceOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<CityAndPriceDto>();
                resultResponse = ConvertDataTable<CityAndPriceDto>(ds.Tables[0]);
                var result = resultResponse.Count > 0 ? resultResponse.SingleOrDefault() : null;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CityAndPriceDto GetCityPriceById(int id)
        {
            try
            {
                var param = new SqlParameter[6];
                param[0] = new SqlParameter("@Id", id);
                param[1] = new SqlParameter("@ToCityId", 0);
                param[2] = new SqlParameter("@FromCityId", 0);
                param[3] = new SqlParameter("@Price", 0);
                param[4] = new SqlParameter("@CompanyId", 0);
                param[5] = new SqlParameter("@OperationType", "GETBYID");

                var cmd = new SqlCommand("prdCityForPriceOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<CityAndPriceDto>();
                resultResponse = ConvertDataTable<CityAndPriceDto>(ds.Tables[0]);
                var result = resultResponse.Count > 0 ? resultResponse.SingleOrDefault() : null;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Yolcu işlemleri
        public int InsertPassenger(PassengerDto model)
        {
            try
            {
                var param = new SqlParameter[10];
                param[0] = new SqlParameter("@Id", model.Id);
                param[1] = new SqlParameter("@Name", model.Name);
                param[2] = new SqlParameter("@Surname", model.Surname);
                param[3] = new SqlParameter("@Age", model.Age);
                param[4] = new SqlParameter("@BirthDate", model.BirthDate);
                param[5] = new SqlParameter("@Tc", model.Tc);
                param[6] = new SqlParameter("@IsTc", model.IsTc);
                param[7] = new SqlParameter("@PassportNo", model.PassportNo);
                param[8] = new SqlParameter("@PassengerAndTicketId", model.PassengerAndTicketId);
                param[9] = new SqlParameter("@OperationType", "INS");

                var cmd = new SqlCommand("prdCityOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool InsertPassengerMultiple(int passengerAndTicketId, List<PassengerAndTicketSaveDto> model)
        {
            try
            {
                foreach (var item in model)
                {
                    var cn = new SqlConnection(connStr);
                    var cmd = new SqlCommand("prdPassengerOperation", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Surname", item.Surname);
                    cmd.Parameters.AddWithValue("@Age", item.Age);
                    cmd.Parameters.AddWithValue("@BirthDate", item.BirthDate);
                    cmd.Parameters.AddWithValue("@Tc", item.Tc);
                    cmd.Parameters.AddWithValue("@IsTc", item.IsTc);
                    cmd.Parameters.AddWithValue("@PassportNo", item.PassportNo);
                    cmd.Parameters.AddWithValue("@PassengerAndTicketId", passengerAndTicketId);
                    cmd.Parameters.AddWithValue("@OperationType", "INS");
                    cn.Open();
                    var result = Convert.ToInt32(cmd.ExecuteScalar());

                    cn.Close();


                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int UpdatePassenger(PassengerDto model)
        {
            try
            {
                var param = new SqlParameter[10];
                param[0] = new SqlParameter("@Id", model.Id);
                param[1] = new SqlParameter("@Name", model.Name);
                param[2] = new SqlParameter("@Surname", model.Surname);
                param[3] = new SqlParameter("@Age", model.Age);
                param[4] = new SqlParameter("@BirthDate", model.BirthDate);
                param[5] = new SqlParameter("@Tc", model.Tc);
                param[6] = new SqlParameter("@IsTc", model.IsTc);
                param[7] = new SqlParameter("@PassportNo", model.PassportNo);
                param[8] = new SqlParameter("@PassengerAndTicketId", model.PassengerAndTicketId);
                param[9] = new SqlParameter("@OperationType", "UPD");

                var cmd = new SqlCommand("prdCityOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool DeletePassenger(PassengerDto model)
        {
            try
            {
                var param = new SqlParameter[10];
                param[0] = new SqlParameter("@Id", model.Id);
                param[1] = new SqlParameter("@Name", model.Name);
                param[2] = new SqlParameter("@Surname", model.Surname);
                param[3] = new SqlParameter("@Age", model.Age);
                param[4] = new SqlParameter("@BirthDate", model.BirthDate);
                param[5] = new SqlParameter("@Tc", model.Tc);
                param[6] = new SqlParameter("@IsTc", model.IsTc);
                param[7] = new SqlParameter("@PassportNo", model.PassportNo);
                param[8] = new SqlParameter("@PassengerAndTicketId", model.PassengerAndTicketId);

                param[9] = new SqlParameter("@OperationType", "DEL");

                var cmd = new SqlCommand("prdCityOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<PassengerDto> GetPassenger(int ticketId = 0)
        {
            try
            {
                var param = new SqlParameter[10];
                param[0] = new SqlParameter("@Id", 0);
                param[1] = new SqlParameter("@Name", "");
                param[2] = new SqlParameter("@Surname", "");
                param[3] = new SqlParameter("@Age", "");
                param[4] = new SqlParameter("@BirthDate", "");
                param[5] = new SqlParameter("@Tc", "");
                param[6] = new SqlParameter("@IsTc", "");
                param[7] = new SqlParameter("@PassportNo", "");
                param[8] = new SqlParameter("@PassengerAndTicketId", ticketId);
                param[9] = new SqlParameter("@OperationType", "SELECT");

                var cmd = new SqlCommand("prdPassengerOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<PassengerDto>();
                resultResponse = ConvertDataTable<PassengerDto>(ds.Tables[0]);
                return resultResponse.Count > 0 ? resultResponse : new List<PassengerDto>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<PassengerDto> GetPassengerByTicketId(int ticketId)
        {
            try
            {
                var param = new SqlParameter[10];
                param[0] = new SqlParameter("@Id", 0);
                param[1] = new SqlParameter("@Name", "");
                param[2] = new SqlParameter("@Surname", "");
                param[3] = new SqlParameter("@Age", "");
                param[4] = new SqlParameter("@BirthDate", "");
                param[5] = new SqlParameter("@Tc", "");
                param[6] = new SqlParameter("@IsTc", "");
                param[7] = new SqlParameter("@PassportNo", "");
                param[8] = new SqlParameter("@PassengerAndTicketId", ticketId);
                param[9] = new SqlParameter("@OperationType", "GETTICKETID");

                var cmd = new SqlCommand("prdPassengerOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<PassengerDto>();
                resultResponse = ConvertDataTable<PassengerDto>(ds.Tables[0]);
                return resultResponse.Count > 0 ? resultResponse : new List<PassengerDto>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Passenger ticket

        public int InsertPassengerAndTicket(PassengerAndTicketDto model)
        {
            try
            {
                var operationType = model.Id > 0 ? "UPD" : "INS";

                var cn = new SqlConnection(connStr);
                var cmd = new SqlCommand("prdPassengerAndTicket", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@ToCityId", model.ToCityId);
                cmd.Parameters.AddWithValue("@FromCityId", model.FromCityId);
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@TotalCount", model.TotalCount);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                cmd.Parameters.AddWithValue("@OperationType", operationType);
                cn.Open();
                var result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool DeletePassengerAndTicket(PassengerAndTicketDto model)
        {
            try
            {
                var param = new SqlParameter[8];
                param[0] = new SqlParameter("@Id", model.Id);
                param[1] = new SqlParameter("@ToCityId", 0);
                param[2] = new SqlParameter("@FromCityId", 0);
                param[3] = new SqlParameter("@UserId", 0);
                param[4] = new SqlParameter("@TotalCount", 0);
                param[5] = new SqlParameter("@CreatedDate", DateTime.Now);
                param[6] = new SqlParameter("@CompanyId", 0);
                param[7] = new SqlParameter("@OperationType", "DEL");

                var cmd = new SqlCommand("prdPassengerAndTicket", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<PassengerAndTicketDto> GetCityPassengerAndTicket()
        {
            try
            {
                var param = new SqlParameter[8];
                param[0] = new SqlParameter("@Id", 0);

                param[1] = new SqlParameter("@ToCityId", 0);
                param[2] = new SqlParameter("@FromCityId", 0);
                param[3] = new SqlParameter("@UserId", 0);
                param[4] = new SqlParameter("@TotalCount", 0);
                param[5] = new SqlParameter("@CreatedDate", DateTime.Now);
                param[6] = new SqlParameter("@CompanyId", 0);
                param[7] = new SqlParameter("@OperationType", "SELECT");

                var cmd = new SqlCommand("prdPassengerAndTicket", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<PassengerAndTicketDto>();
                resultResponse = ConvertDataTable<PassengerAndTicketDto>(ds.Tables[0]);

                foreach (var passengerAndTicketDto in resultResponse)
                {
                    passengerAndTicketDto.PassengerDtos = new List<PassengerDto>();
                    passengerAndTicketDto.PassengerDtos.AddRange(GetPassengerByTicketId(passengerAndTicketDto.Id));
                }

                return resultResponse.Count > 0 ? resultResponse : new List<PassengerAndTicketDto>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Company işlemleri
        public int InsertCompany(CompanyDto model)
        {
            try
            {
                var operationType = model.Id > 0 ? "UPD" : "INS";
                var cn = new SqlConnection(connStr);
                var cmd = new SqlCommand("prdCompanyOperation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@OperationType", operationType);
                cn.Open();
                var result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool DeleteCompany(int id)
        {
            try
            {
                var cn = new SqlConnection(connStr);
                var cmd = new SqlCommand("prdCompanyOperation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", 0);
                cmd.Parameters.AddWithValue("@Description", "");
                cmd.Parameters.AddWithValue("@OperationType", "DEL");
                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<CompanyDto> GetCompany()
        {
            try
            {
                var param = new SqlParameter[4];
                param[0] = new SqlParameter("@Id", 0);
                param[1] = new SqlParameter("@Name", "");
                param[2] = new SqlParameter("@Description", "");
                param[3] = new SqlParameter("@OperationType", "SELECT");

                var cmd = new SqlCommand("prdCompanyOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<CompanyDto>();
                resultResponse = ConvertDataTable<CompanyDto>(ds.Tables[0]);
                return resultResponse.Count > 0 ? resultResponse : new List<CompanyDto>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CompanyDto GetByCompanyId(int id)
        {
            try
            {
                var param = new SqlParameter[4];
                param[0] = new SqlParameter("@Id", id);
                param[1] = new SqlParameter("@Name", "");
                param[2] = new SqlParameter("@Description", "");
                param[3] = new SqlParameter("@OperationType", "GETBYID");

                var cmd = new SqlCommand("prdCompanyOperation", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

                var resultResponse = new List<CompanyDto>();
                resultResponse = ConvertDataTable<CompanyDto>(ds.Tables[0]);
                var result = resultResponse.Count > 0 ? resultResponse.SingleOrDefault() : null;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Dashboard
         public DashboardDto GetDashboard()
        {
            try
            {
                var param = new SqlParameter[0];
              

                var cmd = new SqlCommand("dashboard", new SqlConnection(connStr));
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandTimeout = (1000 * 60 * 10);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);

               var resultResponse = new List<DashboardDto>();
                resultResponse = ConvertDataTable<DashboardDto>(ds.Tables[0]);
                return  resultResponse.Count > 0 ? resultResponse.SingleOrDefault() : null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        object value = dr[column.ColumnName];
                        if (value == DBNull.Value)
                            value = null;
                        pro.SetValue(obj, value, null);
                    }
                    else
                        continue;
                    //if (pro.Name == column.ColumnName)

                    //    pro.SetValue(obj, dr[column.ColumnName], null);
                    //else
                    //    continue;
                }
            }
            return obj;
        }
       
    }
}
