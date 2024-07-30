namespace RestItla.Application.Extras.ResultObject
{
    public struct Unit
    {
        public static readonly Unit T;

        public override bool Equals(object? obj)
        {
            return obj is Unit;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string? ToString()
        {
            return "()";
        }
    }
}