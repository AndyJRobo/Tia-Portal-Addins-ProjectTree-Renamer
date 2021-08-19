namespace ProjectTreeRenamer.Visitors
{
    public interface IVisitor
    {
        void Visit(PlcBlockUserGroupProxy blockGroup);

        void Visit(PlcBlockProxy block);
    }
}