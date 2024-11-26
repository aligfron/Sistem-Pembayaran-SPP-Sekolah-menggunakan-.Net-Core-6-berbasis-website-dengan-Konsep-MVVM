using Newtonsoft.Json;
using System.Net;
using System.Text;
using ViewModel;

namespace SPP_Sekolah.Models
{
    public class ProfilSekolahModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiUrl;
        HttpContent content;
        private string jsonData;

        public ProfilSekolahModel(IConfiguration _config)
        {
            apiUrl = _config["apiUrl"];
        }
        public async Task<VMTbSekolah?> GetAll()
        {
            VMResponse<VMTbSekolah>? apiResponse = null;

            try
            {
                HttpResponseMessage apiResponseMsg = await httpClient.GetAsync($"{apiUrl}ProfilSekolah/GetAll");

                if (apiResponseMsg == null)
                {
                    throw new Exception("School Profile API could not be reached!");
                }

                if (apiResponseMsg.StatusCode == HttpStatusCode.OK)
                {
                    var content = await apiResponseMsg.Content.ReadAsStringAsync();
                    apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbSekolah>>(content);
                }

                // Return null if apiResponse or its Data is null
                return apiResponse?.Data;
            }
            catch (Exception e)
            {
                // Log the error message or handle it accordingly
                throw new Exception($"School Profile.GetAll: {e.Message}");
            }
        }

        public async Task<VMTbSekolah?> getById(int id)
        {
            VMTbSekolah? dataCoba = null;
            try
            {

                VMResponse<VMTbSekolah>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbSekolah>>
                    (await httpClient.GetStringAsync(apiUrl + "ProfilSekolah/" + id));



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
                Console.WriteLine($"SchoolProfileModel.GetById : {ex.Message}");
            }
            return dataCoba;
        }
        public async Task<VMResponse<VMTbSekolah>?> CreateAsync(VMTbSekolah data)
        {
            VMResponse<VMTbSekolah>? apiResponse = new VMResponse<VMTbSekolah>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbSekolah>?>(
                    await httpClient.PostAsync($"{apiUrl}ProfilSekolah", content).Result.Content.ReadAsStringAsync()
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
                    throw new Exception("SchoolProfile api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SchoolProfileModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
        public async Task<VMResponse<VMTbSekolah>?> UpdateAsync(VMTbSekolah data)
        {
            VMResponse<VMTbSekolah>? apiResponse = new VMResponse<VMTbSekolah>();
            try
            {
                //manggil api update proses
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTbSekolah>?>
                    (await httpClient.PutAsync($"{apiUrl}ProfilSekolah", content).Result.Content.ReadAsStringAsync());

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode != HttpStatusCode.OK)
                    {

                        throw new Exception(apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception("SchoolProfile api could not be reached");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"SchoolProfileModel.GetbyId: {e.Message}");

            }
            return apiResponse;
        }
    }
}
