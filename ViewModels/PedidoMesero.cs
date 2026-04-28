namespace Proyecto_CPS.ViewModels
{
    public class PedidoMesero
    {
        public int MesaId { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public List<ItemPedidoVM> Items { get; set; }
    }
    public class ItemPedidoVM
    {
        public int MenuId { get; set; }
        public int Cantidad { get; set; }
    }
}
