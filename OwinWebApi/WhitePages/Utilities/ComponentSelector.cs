using System.Collections.Generic;
using System.Management.Instrumentation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using WhitePages.Enums;

namespace WhitePages.Utilities
{
    public class ComponentSelector
    {
        private readonly Window _window;

        private const double OffsetPercentage = 0.30;

        private readonly double _offset;

        private readonly SearchCriteria _searchCriteria;

        private bool _firstAdjusterInitialized;

        private Label _lable;

        public ComponentSelector(Window window, SearchCriteria searchCriteria)
        {
            _window = window;
            _searchCriteria = searchCriteria;
            _firstAdjusterInitialized = false;
            var root = window.GetParent(searchCriteria);
            _offset = (root.Bounds.Right - root.Bounds.Left) * OffsetPercentage;
            Adjusters = new Dictionary<string, AdjusterContainer>();
        }

        public Dictionary<string, AdjusterContainer> Adjusters { get; }

        public ComponentSelector BuildAdjustersInOrder(string adjusterName)
        {
            _lable = !_firstAdjusterInitialized ? _window.GetNextType<Label>(_searchCriteria) : _lable.GetNextSibling<Label>();
            Adjusters.Add(adjusterName, new AdjusterContainer(_lable));
            _firstAdjusterInitialized = true;
            return this;
        }

        public void ChangeValueByOneClick(string adjusterName, AdjustButton button)
        {
            AdjusterContainer container;
            Adjusters.TryGetValue(adjusterName, out container);
            container?.ChangeValueByOneClick(button, _offset);
        }

        public bool SetValue(string adjusterName, string value)
        {
            AdjusterContainer container;
            Adjusters.TryGetValue(adjusterName, out container);
            if (container == null)
            {
                throw new InstanceNotFoundException(adjusterName);
            }
            return container.SetValue(value, _offset);
        }
    }
}
