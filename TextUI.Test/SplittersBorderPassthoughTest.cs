using System.Collections.Generic;
using TextUI.Interfaces;
using TextUI.Layouts;
using Xunit;

namespace TextUI.Test
{
    public class SplittersBorderPassthoughTest
    {
        [Fact]
        public void HorizontalSplit()
        {
            var ctx = new Moq.Mock<ICanvas>();

            ctx.Setup(c => c.Width).Returns(100);
            ctx.Setup(c => c.Height).Returns(100);

            var left = new Moq.Mock<IBorderFeedback>();
            var right = new Moq.Mock<IBorderFeedback>();

            left.Setup(f => f.Render(Moq.It.IsAny<ICanvas>()))
                .Returns(new Feedback
                {
                    Top =
                    {
                        {1, BorderType.Double},
                        {2, BorderType.Single}
                    },
                    Bottom =
                    {
                        {3, BorderType.Double},
                        {4, BorderType.Single}
                    },

                    Left =
                    {
                        {1, BorderType.Double},
                        {2, BorderType.Single}
                    },
                    Right =
                    {
                        {3, BorderType.Double},
                        {4, BorderType.Single}
                    }

                });

            right.Setup(f => f.Render(Moq.It.IsAny<ICanvas>()))
                .Returns(new Feedback
                {
                    Top =
                    {
                        {10, BorderType.Double},
                        {20, BorderType.Single}
                    },
                    Bottom =
                    {
                        {30, BorderType.Double},
                        {40, BorderType.Single}
                    },

                    Left =
                    {
                        {10, BorderType.Double},
                        {20, BorderType.Single}
                    },
                    Right =
                    {
                        {30, BorderType.Double},
                        {40, BorderType.Single}
                    }

                });

            var lrend = left.As<IRender>();
            var rrend = right.As<IRender>();

            IBorderFeedback x = new SplitLeftRight(lrend.Object, rrend.Object);

            var fb = x.Render(ctx.Object);

            MyAssert.AreEqual(fb.Top, new Dictionary<int, BorderType>
            {
                {1, BorderType.Double},
                {2, BorderType.Single},
                {50 + 10, BorderType.Double},
                {50 + 20, BorderType.Single}
            });

            MyAssert.AreEqual(fb.Bottom, new Dictionary<int, BorderType>
            {
                {3, BorderType.Double},
                {4, BorderType.Single},
                {50 + 30, BorderType.Double},
                {50 + 40, BorderType.Single}
            });

            MyAssert.AreEqual(fb.Left, new Dictionary<int, BorderType>
            {
                {1, BorderType.Double},
                {2, BorderType.Single}
            });
            MyAssert.AreEqual(fb.Right, new Dictionary<int, BorderType>
            {
                {30, BorderType.Double},
                {40, BorderType.Single}
            });
        }

        [Fact]
        public void VerticalSplit()
        {
            var ctx = new Moq.Mock<ICanvas>();

            ctx.Setup(c => c.Width).Returns(100);
            ctx.Setup(c => c.Height).Returns(100);

            var top = new Moq.Mock<IBorderFeedback>();
            var bottom = new Moq.Mock<IBorderFeedback>();

            top.Setup(f => f.Render(Moq.It.IsAny<ICanvas>()))
                .Returns(new Feedback
                {
                    Top =
                    {
                        {1, BorderType.Double},
                        {2, BorderType.Single}
                    },
                    Bottom =
                    {
                        {3, BorderType.Double},
                        {4, BorderType.Single}
                    },

                    Left =
                    {
                        {1, BorderType.Double},
                        {2, BorderType.Single}
                    },
                    Right =
                    {
                        {3, BorderType.Double},
                        {4, BorderType.Single}
                    }

                });

            bottom.Setup(f => f.Render(Moq.It.IsAny<ICanvas>()))
                .Returns(new Feedback
                {
                    Top =
                    {
                        {10, BorderType.Double},
                        {20, BorderType.Single}
                    },
                    Bottom =
                    {
                        {30, BorderType.Double},
                        {40, BorderType.Single}
                    },

                    Left =
                    {
                        {10, BorderType.Double},
                        {20, BorderType.Single}
                    },
                    Right =
                    {
                        {30, BorderType.Double},
                        {40, BorderType.Single}
                    }
                });

            var trend = top.As<IRender>();
            var brend = bottom.As<IRender>();

            IBorderFeedback x = new SpliTopBottom(trend.Object, brend.Object);

            var fb = x.Render(ctx.Object);

            MyAssert.AreEqual(fb.Top, new Dictionary<int, BorderType>
            {
                {1, BorderType.Double},
                {2, BorderType.Single}
            });

            MyAssert.AreEqual(fb.Bottom, new Dictionary<int, BorderType>
            {

                {30, BorderType.Double},
                {40, BorderType.Single}
            });

            MyAssert.AreEqual(fb.Left, new Dictionary<int, BorderType>
            {
                {1, BorderType.Double},
                {2, BorderType.Single},
                {50 + 10, BorderType.Double},
                {50 + 20, BorderType.Single}
            });
            MyAssert.AreEqual(fb.Right, new Dictionary<int, BorderType>
            {
                {3, BorderType.Double},
                {4, BorderType.Single},
                {50 + 30, BorderType.Double},
                {50 + 40, BorderType.Single}
            });
        }
    }

    public static class MyAssert
    {
        public static void AreEqual<TKey, TValue>(Dictionary<TKey, TValue> actual, Dictionary<TKey, TValue> expected)
        {
            Assert.Equal(expected.Count, actual.Count);

            foreach (var kv in expected)
            {
                Assert.True(actual.ContainsKey(kv.Key), "Key " + kv.Key + " is missing");
                Assert.Equal(kv.Value, actual[kv.Key], EqualityComparer<TValue>.Default);
            }
        }
    }
}
