using System.Collections.Generic;
using System.ComponentModel;

namespace Summer.System.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged, ICommandExecutor
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Dictionary<string, object> variables = new Dictionary<string, object>();

        public void SetValue(string name, object value)
        {
            if (variables.ContainsKey(name) && variables[name] == value)
                //表示没有修改
                return;



            variables[name] = value;
            RaisePropertyChanged(name);
        }

        public object GetValue(string name)
        {
            object v;
            variables.TryGetValue(name, out v);
            return v;
        }

        public virtual void ExexuteCommand(object src, string cmdType, params object[] cmdParams)
        {

        }

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
