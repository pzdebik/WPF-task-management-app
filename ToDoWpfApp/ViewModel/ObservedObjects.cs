using System.ComponentModel;

// klasa implementuje interfejs INotifyPropertyChanged
public abstract class ObservedObjects : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void onPropertyChanged(params string[] names)
    {
        if (PropertyChanged != null)
        {
            foreach(string name in names)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
