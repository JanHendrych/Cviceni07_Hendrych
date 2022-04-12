using System.Collections;

namespace ChampionsLeagueLibrary
{
    public class ObjectLinkedList : ICollection, IEnumerable, IList
    {
        Prvek? prvni = null;
        Prvek? posledni = null;
        class Prvek
        {
            public Prvek? predchozi;
            public Prvek? nasledujici;
            public Object? data;

            public Prvek(object data)
            {
                this.data = data;
                predchozi = null;
                nasledujici = null;
            }
        }
        public object? this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    return null;
                }
                Prvek? temp = prvni;
                for (int i = 0; i < index; i++)
                {
                    temp = temp.nasledujici;
                }
                return temp?.data;
            }
            set
            {
                Prvek prvek = new Prvek(value);
                if (index >= 0 && index < Count)
                {
                    Prvek? temp = prvni;
                    for (int i = 0; i < index; i++)
                    {
                        temp = temp?.nasledujici;
                    }
                    Prvek? predchozi;
                    Prvek? nasledujici;
                    if (temp.predchozi is not null && temp.nasledujici is not null)
                    {
                        predchozi = temp.predchozi;
                        nasledujici = temp.nasledujici;
                        predchozi.nasledujici = prvek;
                        nasledujici.predchozi = prvek;
                        prvek.nasledujici = nasledujici;
                        prvek.predchozi = predchozi;

                    }
                    else if (temp.predchozi is null && temp.nasledujici is null)
                    {
                        prvni = prvek;
                        posledni = prvek;
                    }
                    else if (temp.predchozi is null)
                    {
                        nasledujici = temp.nasledujici;
                        nasledujici.predchozi = prvek;
                        prvek.nasledujici = nasledujici;
                        prvni = prvek;
                    }
                    else if (temp.nasledujici is null)
                    {
                        predchozi = temp.predchozi;
                        predchozi.nasledujici = prvek;
                        prvek.predchozi = predchozi;
                        posledni = prvek;
                    }
                    temp = null;
                }
            }
        }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public int Count
        {
            get
            {
                int pocet = 0;
                IEnumerator enumerator = this.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    pocet++;
                }
                return pocet;
            }
        }

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public int Add(object? value)
        {
            Prvek novyPrvek = new Prvek(value);
            if (prvni is null)
            {
                prvni = novyPrvek;
                posledni = novyPrvek;
                return 0;
            }
            else
            {
                posledni.nasledujici = novyPrvek;
                novyPrvek.predchozi = posledni;
                posledni = novyPrvek;
                return IndexOf(value);
            }
        }

        public void Clear()
        {
            prvni = posledni = null;
        }

        public bool Contains(object? value)
        {
            IEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            if (index >= 0 && index < Count)
            {
                int i = 0;
                IEnumerator enumerator = this.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    array.SetValue(enumerator.Current, (i++) + index);
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            Prvek? temp = prvni;
            while (temp is not null)
            {
                Prvek prvek = temp;
                temp = temp.nasledujici;
                yield return prvek.data;
            }
        }

        public int IndexOf(object? value)
        {
            int index = 0;
            IEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(value))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public void Insert(int index, object? value)
        {
            if (index >= 0 && index <= Count)
            {
                if (prvni is null && index == 0)
                {
                    Add(value);
                }
                else if (index == Count)
                {
                    Add(value);
                }
                else
                {
                    Prvek novyPrvek = new Prvek(value);
                    Prvek? temp = prvni;
                    for (int i = 0; i < index; i++)
                    {
                        temp = temp?.nasledujici;
                    }
                    Prvek? predchozi;
                    Prvek? nasledujici;
                    if (temp.predchozi is not null && temp.nasledujici is not null)
                    {
                        /*
                        predchozi = temp.predchozi;
                        nasledujici = temp.nasledujici;

                        predchozi.nasledujici = novyPrvek;
                        nasledujici.predchozi = novyPrvek;
                        novyPrvek.predchozi = predchozi;
                        novyPrvek.nasledujici = nasledujici;
                        */
                        predchozi = temp.predchozi;

                        predchozi.nasledujici = novyPrvek;
                        temp.predchozi = novyPrvek;
                        novyPrvek.predchozi = predchozi;
                        novyPrvek.nasledujici = temp;
                    }
                    else if (temp.predchozi is null)
                    {
                        Prvek docasny = prvni;
                        docasny.predchozi = novyPrvek;
                        novyPrvek.nasledujici = docasny;
                        prvni = novyPrvek;
                    }
                    else if (temp.nasledujici is null)
                    {
                        predchozi = temp.predchozi;

                        predchozi.nasledujici = novyPrvek;
                        novyPrvek.predchozi = predchozi;
                        posledni = novyPrvek;
                    }
                    temp = null;
                }
            }
        }

        public void Remove(object? value)
        {
            if (Contains(value))
            {
                RemoveAt(IndexOf(value));
                /*
                IEnumerator enumerator = GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Prvek temp = new Prvek(enumerator.Current);
                    if (temp.data.Equals(value))
                    {
                        Prvek? predchozi;
                        Prvek? nasledujici;
                        if (temp.predchozi is not null && temp.nasledujici is not null)
                        {
                            predchozi = temp.predchozi;
                            nasledujici = temp.nasledujici;
                            predchozi.nasledujici = nasledujici;
                            nasledujici.predchozi = predchozi;
                        }
                        else if (temp.predchozi is null && temp.nasledujici is null)
                        {
                            prvni = null;
                            posledni = null;
                        }
                        else if (temp.predchozi is null)
                        {
                            nasledujici = temp.nasledujici;
                            nasledujici.predchozi = null;
                            prvni = nasledujici;
                        }
                        else if (temp.nasledujici is null)
                        {
                            predchozi = temp.predchozi;
                            predchozi.nasledujici = null;
                            posledni = predchozi;
                        }
                        temp = null;
                    }
                }
                */
            }
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < Count)
            {
                Prvek? temp = prvni;
                for (int i = 0; i < index; i++)
                {
                    temp = temp?.nasledujici;
                }
                Prvek? predchozi;
                Prvek? nasledujici;
                if (temp.predchozi is not null && temp.nasledujici is not null)
                {
                    predchozi = temp.predchozi;
                    nasledujici = temp.nasledujici;
                    predchozi.nasledujici = nasledujici;
                    nasledujici.predchozi = predchozi;
                }
                else if (temp.predchozi is null && temp.nasledujici is null)
                {
                    prvni = null;
                    posledni = null;
                }
                else if (temp.predchozi is null)
                {
                    nasledujici = temp.nasledujici;
                    nasledujici.predchozi = null;
                    prvni = nasledujici;
                }
                else if (temp.nasledujici is null)
                {
                    predchozi = temp.predchozi;
                    predchozi.nasledujici = null;
                    posledni = predchozi;
                }
                temp = null;
            }
        }
    }
}