using Moq;
using TextUI.Extensions;
using TextUI.Interfaces;
using Xunit;

namespace TextUI.Test
{
    public class JoinsTest
    {
        [Fact]
        public void JoinsArePropagatedToParent()
        {
            var ctx = new Mock<IRenderContext>();

            ctx.Setup(c => c.Width).Returns(100);
            ctx.Setup(c => c.Height).Returns(100);

            var scope = ctx.Object.Scope(20, 30, 20, 20, Side.All);
            scope.JoinLeft(5, BorderType.Double);
            scope.JoinTop(5, BorderType.Single);

            ctx.Verify(c => c.JoinLeft(35, BorderType.Double));
            ctx.Verify(c => c.JoinTop(25, BorderType.Single));
        }
    }
}