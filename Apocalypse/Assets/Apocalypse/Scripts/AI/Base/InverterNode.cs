namespace Apocalypse.AI.Base
{
    public class InverterNode : Node
    {
        private Node _child;

        public InverterNode(Node child)
        {
            _child = child;
        }

        public override NodeState Evaluate()
        {
            switch (_child.Evaluate())
            {
                case NodeState.FAILURE:
                    return NodeState.SUCCESS;
                case NodeState.SUCCESS:
                    return NodeState.FAILURE;
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                default:
                    return NodeState.FAILURE;
            }
        }
    }
}
