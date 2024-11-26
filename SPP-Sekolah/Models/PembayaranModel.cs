using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ViewModel;

namespace SPP_Sekolah.Models
{
    public class PembayaranModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTbMSiswa>>? apiResponse;
        private VMResponse<List<VMTbTPembayaran>>? apiResponse1;
        private string jsonData;

        HttpContent content;

        public PembayaranModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }
        public async Task<List<VMTbMSiswa>?> GetByJurusandanKelas(int jurusanId, int kelasId, string? filter)
        {
            List<VMTbMSiswa>? dataCoba = null;
            try
            {
                // Buat URL dengan pemisah yang benar untuk jurusan, kelas, dan filter
                string requestUrl = $"{apiurl}Siswa/GetByJurusandanKelas/{jurusanId}/{kelasId}/{filter}";



                // Ambil respons dari API dan deserialisasi
                string responseString = await httpClient.GetStringAsync(requestUrl);
                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTbMSiswa>>?>(responseString);

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode == HttpStatusCode.OK)
                    {
                        dataCoba = apiResponse.Data;
                    }
                    else
                    {
                        throw new Exception(apiResponse.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching data: {ex.Message}");
            }
            return dataCoba;
        }
        public async Task<List<VMTbTPembayaran?>> getByIdSiswa(int id)
        {
            List<VMTbTPembayaran>? dataCoba = null;
            try
            {
                apiResponse1 = JsonConvert.DeserializeObject<VMResponse<List<VMTbTPembayaran>>?>
                    (await httpClient.GetStringAsync(apiurl + "Pembayaran/" + id));
                if (apiResponse1 != null)
                {
                    if (apiResponse1.StatusCode == HttpStatusCode.OK)
                    {
                        dataCoba = apiResponse1.Data;
                    }
                    else
                    {
                        throw new Exception(apiResponse1.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PaymentModel.GetById : {ex.Message}");
            }
            return dataCoba;
        }
        public async Task<VMResponse<VMTbTPembayaran>?> CreateAsync(VMTbTPembayaran data)
        {
            VMResponse<VMTbTPembayaran>? apiResponse = new VMResponse<VMTbTPembayaran>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbTPembayaran>?>(
                    await httpClient.PostAsync($"{apiurl}Pembayaran", content).Result.Content.ReadAsStringAsync()
                    );

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode != HttpStatusCode.Created)
                    {
                        throw new Exception(apiResponse.Message);
                    }

                }
                else
                {
                    throw new Exception("Payment api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PaymentModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
        public async Task<VMTbTPembayaran?> getById(int id)
        {
            VMTbTPembayaran? dataCoba = null;
            try
            {
                VMResponse<VMTbTPembayaran>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbTPembayaran>>(await httpClient.GetStringAsync(apiurl + "Pembayaran/GetById/" + id));

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode == HttpStatusCode.OK)
                    {
                        dataCoba = apiResponse.Data;
                    }
                    else
                    {
                        throw new Exception(apiResponse.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClassModel.GetById : {ex.Message}");
            }
            return dataCoba;
        }
    }
}
