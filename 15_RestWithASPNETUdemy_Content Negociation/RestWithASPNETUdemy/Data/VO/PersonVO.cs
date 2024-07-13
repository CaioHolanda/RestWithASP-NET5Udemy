using RestWithASPNETUdemy.Model.Base;
using System.Text.Json.Serialization;

namespace RestWithASPNETUdemy.Data.VO
{
    public class PersonVO
    {
        [JsonPropertyName("Índice")]
        public long Id { get; set; }
        [JsonPropertyName("Primeiro Nome")]
        public string FirstName { get; set; }
        [JsonPropertyName("Sobrenome")]
        public string LastName { get; set; }
        [JsonPropertyName("Endereço")]
        public string Address { get; set; }
        
        public string Gender { get; set; }
    }
}
