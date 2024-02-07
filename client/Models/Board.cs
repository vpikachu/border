namespace boarder.client.Models {
public class Board {
  public int Id {get;set;}
  public string Name{get;set;}
  public string CreatedBy {get;set;}  
  public long CreatedTicks{get{ return _CreatedDate.Ticks;}
  set{    
    _CreatedDate = new DateTime(value);
  }
  }
  private DateTime _CreatedDate = DateTime.Now;
  public  DateTime CreatedDate{
    get{return _CreatedDate;}    
  }
  public Board()
  {
    Name = "";
    CreatedBy = "";

  }
}
}