using System;

namespace Tower
{
    public class TowerAttribute
    {
        bool value;
        public int Index { get; set; }
        public delegate void ChangeAction( int index,bool value);
        public event ChangeAction OnValueChanged;
        public bool Value
        {
            get => value;
            set
            {
                if (this.value == value) return;
                
                this.value = value;
                OnValueChanged?.Invoke(Index,value);
            }
        }
    }
}