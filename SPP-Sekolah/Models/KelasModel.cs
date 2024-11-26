using Newtonsoft.Json;
using System.Net;
using System.Text;
using ViewModel;

namespace SPP_Sekolah.Models
{
    public class KelasModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTbMKela>>? apiResponse;
        private string jsonData;

        HttpContent content;

        public KelasModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }
        public async Task<List<VMTbMKela>>? getByFilter(string? filter)
        {
            List<VMTbMKela>? dataCoba = null;
            try
            {
                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTbMKela>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiurl + "Kelas")
                    : await httpClient.GetStringAsync(apiurl + "Kelas/GetBy/" + filter));

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
        public async Task<List<VMTbMKela>>? GetByJurusanId(int? jurusanid)
        {
            List<VMTbMKela>? dataCoba = null;
            try
            {

                VMResponse<List<VMTbMKela>>? apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTbMKela>>?>
                    (await httpClient.GetStringAsync($"{apiurl}Kelas/GetByJurusanId/{jurusanid}"));
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
        public async Task<VMResponse<VMTbMKela>?> DeleteAsync(int id, int userId)
        {
            VMResponse<VMTbMKela>? apiResponse = new VMResponse<VMTbMKela>();
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMKela>?>(
                    await httpClient.DeleteAsync($"{apiurl}Kelas/{id}/{userId}").Result.Content.ReadAsStringAsync()
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
                    throw new Exception("Class api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClassModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
        public async Task<VMTbMKela?> getById(int id)
        {
            VMTbMKela? dataCoba = null;
            try
            {
                VMResponse<VMTbMKela>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMKela>>(await httpClient.GetStringAsync(apiurl + "Kelas/" + id));

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

        public async Task<VMResponse<VMTbMKela>?> UpdateAsync(VMTbMKela data)
        {
            VMResponse<VMTbMKela>? apiResponse = new VMResponse<VMTbMKela>();
            try
            {
                //manggil api update proses
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMKela>?>
                    (await httpClient.PutAsync($"{apiurl}Kelas", content).Result.Content.ReadAsStringAsync());

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

        public async Task<VMResponse<VMTbMKela>?> CreateAsync(VMTbMKela data)
        {
            VMResponse<VMTbMKela>? apiResponse = new VMResponse<VMTbMKela>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbMKela>?>(
                    await httpClient.PostAsync($"{apiurl}Kelas", content).Result.Content.ReadAsStringAsync()
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
                    throw new Exception("Class api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClassModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
    }
}
