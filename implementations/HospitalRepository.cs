using System.Text.Json;
using Microsoft.Extensions.Options;

namespace surgical_reports.implementations;

public class HospitalRepository : IHospitalRepository
{

    private IMapper _map;
    private IOptions<ComSettings> _com;

    public HospitalRepository(DapperContext context, IMapper map, IOptions<ComSettings> com)
    {
        _map = map;
        _com = com;
    }

    public async Task<HospitalForReturnDTO> GetSpecificHospital(string id)
    {
        var hospitalNo = id.makeSureTwoChar();
        var comaddress = _com.Value.hospitalURL;
        var st = "Hospital/" + hospitalNo;
        comaddress = comaddress + st;
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync(comaddress))
            {
                string help = await response.Content.ReadAsStringAsync();
                if (help != "")
                {
                    var res = JsonSerializer.Deserialize<Class_Hospital>(help);
                    var result = _map.Map<HospitalForReturnDTO>(res);
                    return result;
                }
                else { return null; }
            }
        }
    }
}
