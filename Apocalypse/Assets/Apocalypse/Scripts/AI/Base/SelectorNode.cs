using System.Collections.Generic;

namespace Apocalypse.AI.Base
{
    public class SelectorNode : Node
    {
        // Danh sách các con nút
        protected List<Node> children = new List<Node>();

        public SelectorNode(List<Node> children)
        {
            this.children = children;
        }

        public override NodeState Evaluate()
        {
            // Duyệt qua tất cả các con nút
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        // Nếu con nút thất bại, tiếp tục với nút tiếp theo
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS; // Nếu con nút thành công, trả về thành công
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING; // Nếu con nút đang chạy, trả về trạng thái đang chạy
                        return state;
                    default: continue;
                }
            }

            state = NodeState.FAILURE; // Mặc định là thất bại
            return state;
        }
    }
}