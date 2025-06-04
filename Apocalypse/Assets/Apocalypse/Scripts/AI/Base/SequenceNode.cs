using System.Collections.Generic;

namespace Apocalypse.AI.Base
{
    public class SequenceNode : Node
    {
        // Danh sách các con nút
        protected List<Node> children = new List<Node>();

        public SequenceNode(List<Node> children)
        {
            this.children = children;
        }

        public override NodeState Evaluate()
        {
            var anyChildRunning = false; // Biến để kiểm tra xem có con nút nào đang chạy hay không
                                         // Duyệt qua tất cả các con nút
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE; // Nếu con nút thất bại, trả về thất bại
                        return state;
                    case NodeState.SUCCESS:
                        continue; // Nếu con nút thành công, tiếp tục với nút tiếp theo
                    case NodeState.RUNNING:
                        anyChildRunning = true; // Nếu con nút đang chạy, đánh dấu là đang chạy
                        continue;
                    default:
                        state = NodeState.FAILURE; // Mặc định là thất bại
                        return state;
                }
            }

            state = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            // Nếu có con nút đang chạy, trả về RUNNING, nếu không thì SUCCESS
            return state;
        }
    }
}