using ClienteApiFinal.Services.Model;
using ClienteApiFinal.Dtos;
using ClienteApiFinal.Models;
using Riok.Mapperly.Abstractions;

namespace ClienteApiFinal.Mappers
{
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public partial class ClienteMapper
    {
        public partial ClienteDTO ClienteToClienteDto(Cliente cliente);

        public partial List<ClienteDTO> ClienteToClienteDto(List<Cliente> cliente);

        public partial Cliente ClienteDtoToCliente(ClienteDTO cliente);

        public partial List<Cliente> ClienteDtoToCliente(List<ClienteDTO> cliente);

        [MapProperty(nameof(EnderecoCep.Localidade), nameof(EnderecoDTO.Cidade))]
        public partial EnderecoDTO EnderecoCepToEnderecoDto(EnderecoCep enderecoCep);
    }
}
