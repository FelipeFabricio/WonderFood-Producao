namespace WonderFood.ExternalServices;

public class ExternalServicesSettings
{
    public WonderfoodPedidos WonderfoodPedidos { get; set; }
}

public class WonderfoodPedidos
{
    public string BaseUrl { get; set; }
    public string AlteracaoStatus { get; set; }
}