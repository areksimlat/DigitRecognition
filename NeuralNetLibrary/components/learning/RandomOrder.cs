using System;
using System.Collections.Generic;

namespace NeuralNetLibrary.components.learning
{
    public class RandomOrder
    {
        private List<int> order;

        public RandomOrder(List<DataItem> items, DataItem.DataTypes dataType)
        {
            initOrder(items, dataType);
        }

        private void initOrder(List<DataItem> items, DataItem.DataTypes dataType)
        {
            order = new List<int>();

            for (int i = 0; i < items.Count; i++)
                if (items[i].dataType == dataType)
                    order.Add(i);
        }

        public List<int> GetShuffledOrder()
        {
            shuffleOrder();
            return order;
        }

        private void shuffleOrder()
        {
            Random rand = new Random();

            for (int i = 0; i < order.Count; i++)
            {
                int randIndex = rand.Next(order.Count);

                int temp = order[i];
                order[i] = order[randIndex];
                order[randIndex] = temp;
            }
        }
    }
}
