namespace ProjectTreeRenamer.Visitors
{
    public interface IElement
    {
        void Accept(IVisitor visitor);
    }
}