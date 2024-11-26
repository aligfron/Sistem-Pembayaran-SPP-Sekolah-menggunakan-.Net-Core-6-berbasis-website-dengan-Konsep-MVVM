using Newtonsoft.Json;
using System.Net;
using System.Text;
using ViewModel;

namespace SPP_Sekolah.Models
{
    public class SiswaModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTbMSiswa>>? apiResponse;
        private string jsonData;

        HttpContent content;

        public SiswaModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }
        public async Task<List<VMTbMSiswa>>? getByFilter(string? filter)
        {
            List<VMTbMSiswa>? dataCoba = null;
            try
            {
                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTbMSiswa>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiurl + "Siswa")
                    : await httpClient.GetStringAsync(apiurl + "Siswa/GetBy/" + filter));

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
                throw new Exception(ex.Message);
            }
            return dataCoba;
        }
        public async Task<VMResponse<VMTbMBiodatum>?> DeleteAsync(int id, int userId)
        {
            VMResponse<VMTbMBiodatum>? apiResponse = new VMResponse<VMTbMBiodatum>();
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMBiodatum>?>(
                    await httpClient.DeleteAsync($"{apiurl}Siswa/{id}/{userId}").Result.Content.ReadAsStringAsync()
                    );
                /* apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMKela>?>(
                     await httpClient.DeleteAsync($"{apiurl}Category?id={id}&userId={userId}").Result.Content.ReadAsStringAsync()
                     );*/

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(apiResponse.Message);
                    }

                }
                else
                {
                    throw new Exception("Student api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StudentModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
        public async Task<VMTbMSiswa?> getById(int id)
        {
            VMTbMSiswa? dataCoba = null;
            try
            {
                VMResponse<VMTbMSiswa>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMSiswa>>(await httpClient.GetStringAsync(apiurl + "Siswa/" + id));

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

        public async Task<VMResponse<VMTbMUser>?> UpdateAsync(VMTbMUser data)
        {
            VMResponse<VMTbMUser>? apiResponse = new VMResponse<VMTbMUser>();
            try
            {
                //manggil api update proses
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMUser>?>
                    (await httpClient.PutAsync($"{apiurl}Siswa", content).Result.Content.ReadAsStringAsync());

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode != HttpStatusCode.OK)
                    {

                        throw new Exception(apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception("Class api could not be reached");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"ClassModel.GetbyId: {e.Message}");

            }
            return apiResponse;
        }

        public async Task<VMResponse<VMTbMUser>?> CreateAsync(VMTbMUser data)
        {
            VMResponse<VMTbMUser>? apiResponse = new VMResponse<VMTbMUser>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMUser>?>(
                    await httpClient.PostAsync($"{apiurl}Siswa", content).Result.Content.ReadAsStringAsync()
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
                    throw new Exception("Student api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StudentModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
    }
}
