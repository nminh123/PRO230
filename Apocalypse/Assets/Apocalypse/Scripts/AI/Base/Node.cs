namespace Apocalypse.AI.Base
{
    // Danh sách các trạng thái của Node
    public enum NodeState
    {
        RUNNING, // đang chạy
        SUCCESS, // thành công
        FAILURE // thất bại
    }

    // Lớp Node đại diện cho một nút trong cây hành vi
    public abstract class Node
    {
        protected NodeState state;
        public NodeState CurrentState => state;
        public abstract NodeState Evaluate();
    }
}