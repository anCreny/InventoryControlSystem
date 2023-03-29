namespace ICSServerApp;

public class UserHandlerService
{
    private int _id;

    public void SetId(int id)
    {
        _id = id;
    }

    public int Id => _id; 
}