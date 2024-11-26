using Newtonsoft.Json;
using System.Net;
using System.Text;
using ViewModel;

namespace SPP_Sekolah.Models
{
    public class JurusanModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTbMJurusan>>? apiResponse;
        private string jsonData;

        HttpContent content;

        public JurusanModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }
        public async Task<List<VMTbMJurusan>>? getByFilter(string? filter)
        {
            List<VMTbMJurusan>? dataCoba = null;
            try
            {
                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTbMJurusan>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiurl + "Jurusan")
                    : await httpClient.GetStringAsync(apiurl + "Jurusan/GetBy/" + filter));

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
        public async Task<VMResponse<VMTbMJurusan>?> DeleteAsync(int id, int userId)
        {
            VMResponse<VMTbMJurusan>? apiResponse = new VMResponse<VMTbMJurusan>();
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMJurusan>?>(
                    await httpClient.DeleteAsync($"{apiurl}Jurusan/{id}/{userId}").Result.Content.ReadAsStringAsync()
                    );
                /* apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMJurusan>?>(
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
                    throw new Exception("Major api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MajorModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
        public async Task<VMTbMJurusan?> getById(int id)
        {
            VMTbMJurusan? dataCoba = null;
            try
            {
                VMResponse<VMTbMJurusan>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMJurusan>>(await httpClient.GetStringAsync(apiurl + "Jurusan/" + id));

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
                Console.WriteLine($"JurusanModel.GetById : {ex.Message}");
            }
            return dataCoba;
        }

        public async Task<VMResponse<VMTbMJurusan>?> UpdateAsync(VMTbMJurusan data)
        {
            VMResponse<VMTbMJurusan>? apiResponse = new VMResponse<VMTbMJurusan>();
            try
            {
                //manggil api update proses
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMJurusan>?>
                    (await httpClient.PutAsync($"{apiurl}Jurusan", content).Result.Content.ReadAsStringAsync());

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode != HttpStatusCode.OK)
                    {

                        throw new Exception(apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception("Major api could not be reached");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"MajorModel.GetbyId: {e.Message}");

            }
            return apiResponse;
        }

        public async Task<VMResponse<VMTbMJurusan>?> CreateAsync(VMTbMJurusan data)
        {
            VMResponse<VMTbMJurusan>? apiResponse = new VMResponse<VMTbMJurusan>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMJurusan>?>(
                    await httpClient.PostAsync($"{apiurl}Jurusan", content).Result.Content.ReadAsStringAsync()
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
                    throw new Exception("Major api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MajorModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
    }
}
