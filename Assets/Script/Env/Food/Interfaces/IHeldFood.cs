public interface IHeldFood
{
   public void ICooked(int cookValue);
   
   public bool IsCooked();
   public void ICutted();
   
   public void IBasicMaterial();


   int _isCooked { get; set; }
}
