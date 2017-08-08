using System;
using System.Reflection;

namespace ExpressionTreeIssue.Controllers
{
    public class Segment
    {
        private readonly StepType _type;
        private readonly PropertyInfo _info;
        public readonly int? _elementId;

        private Segment(StepType type, PropertyInfo info, int? elementId) {
            _elementId = elementId;
            _info = info;
            _type = type;
        }

        public static Segment Property(PropertyInfo pInfo) {
            return new Segment(StepType.Property, pInfo, null);
        }

        public static Segment Collection(PropertyInfo pInfo, int elementId) {
            return new Segment(StepType.Collection, pInfo, elementId);
        }

        public override string ToString() {
            switch (_type) {
                case StepType.Property:
                    return _info.Name;
                case StepType.Collection:
                    return $"{_info.Name}[{_elementId.Value}]";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum StepType
        {
            Property = 1,
            Collection = 2
        }
    }
}