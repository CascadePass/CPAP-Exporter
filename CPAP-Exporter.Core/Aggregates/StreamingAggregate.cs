namespace CascadePass.CPAPExporter.Core.Aggregates
{
    public abstract class StreamingAggregate<T>
    {
        public abstract void Sample(T value);

        public abstract T Value { get; }

        public abstract int Count { get; }

        public abstract void Reset();
    }

    public class StreamingAverage : StreamingAggregate<double>
    {
        private double sum;
        private int count;

        public override void Sample(double value)
        {
            this.sum += value;
            this.count++;
        }

        public override double Value
        {
            get
            {
                if (this.count == 0)
                {
                    return 0;
                }

                return this.sum / this.count;
            }
        }

        public override int Count => this.count;

        public override void Reset()
        {
            this.sum = 0;
            this.count = 0;
        }
    }
}
