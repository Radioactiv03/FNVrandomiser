namespace Randomiser.Filtering
{
    internal interface IFilterPredicate
    {
        bool Match(string value);
    }
}
