namespace GothicModComposer.UI.Models
{
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToString() => $"{Width}x{Height}";

        public override bool Equals(object obj)
        {
            if (obj is not Resolution resolution)
                return false;

            return resolution.Height == Height && resolution.Width == Width;
        }

        public override int GetHashCode() => Height.GetHashCode() + Width.GetHashCode();
    }
}