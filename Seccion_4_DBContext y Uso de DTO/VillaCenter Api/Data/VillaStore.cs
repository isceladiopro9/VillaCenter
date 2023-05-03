using VillaCenter_Api.Models.DTO;

namespace VillaCenter_Api.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{ Id = 1, Nombre="Vista a la calle",Ocupantes=2,MetrosCuadrados=100},
            new VillaDto{ Id = 2,Nombre= "Vista a la playa",Ocupantes=4,MetrosCuadrados=150}
        }; 

    }
}
