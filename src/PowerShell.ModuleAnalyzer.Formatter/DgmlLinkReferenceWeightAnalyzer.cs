using OpenSoftware.DgmlTools.Analyses;
using OpenSoftware.DgmlTools.Model;

namespace ModuleAnalyzer.Formatter;

internal class DgmlLinkReferenceWeightAnalyzer : IGraphAnalysis
{
    public void Execute(DirectedGraph graph)
    {

    }

    public IEnumerable<Property> GetProperties(DirectedGraph graph)
    {
        return new List<Property> {
            new Property
            {
                Id = "RefWeight",
                DataType = "System.Int32",
                Label = "RefWeight",
                Description = "RefWeight"
            },
            new Property
            {
                Id = "Description",
                DataType = "System.Int32",
                Label = "Description",
                Description = "Description"
            }
        };
    }

    public IEnumerable<Style> GetStyles(DirectedGraph graph)
    {
        return new List<Style> {
            new Style
            {
                TargetType = "Link",
                Condition = new List<Condition>
                {
                    new Condition
                    {
                        Expression = "Description = 1"
                    }
                },
                Setter = new List<Setter>
                {
                    new Setter
                    {
                        Property = "Stroke",
                        Value =  "#FFDA3471"
                    }
                }
            },
            new Style
            {
                TargetType = "Link",
                Condition = new List<Condition>
                {
                    new Condition
                    {
                        Expression = "Description > 5"
                    }
                },
                Setter = new List<Setter>
                {
                    new Setter
                    {
                        Property = "Stroke",
                        Value =  "#FFEE91C1"
                    }
                }
            },
            new Style
            {
                TargetType = "Link",
                Condition = new List<Condition>
                {
                    new Condition
                    {
                        Expression = "Description > 10"
                    }
                },
                Setter = new List<Setter>
                {
                    new Setter
                    {
                        Property = "Stroke",
                        Value =  "#FF4DC882"
                    }
                }
            }
        };
    }
}
