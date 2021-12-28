string[] lines = File.ReadLines("input.txt").ToArray();

byte[] imageEnhancementAlgorithm = lines[0].Select(c => c == '#' ? (byte)1 : (byte)0).ToArray();

byte[,] image = ParseImage(lines.Skip(2).ToArray());

const int numberOfIterations = 50;

byte[,] enhancedImage = ExpandImage(image, numberOfIterations * 5);

for (int i = 0; i < numberOfIterations; i++)
{
    enhancedImage = EnhanceImage(enhancedImage, imageEnhancementAlgorithm);
}

int numberOfLitPixels = 0;

for (int i = 3; i <= enhancedImage.GetUpperBound(0) - 3; i++)
{
    for (int j = 3; j <= enhancedImage.GetUpperBound(1) - 3; j++)
    {
        if (enhancedImage[i, j] == 1)
        {
            numberOfLitPixels++;
        }
    }
}

Console.WriteLine(numberOfLitPixels);

static byte[,] ParseImage(string[] lines)
{
    byte[,] image = new byte[lines.Length, lines[0].Length];

    for (int i = 0; i <= image.GetUpperBound(0); i++)
    {
        for (int j = 0; j <= image.GetUpperBound(1); j++)
        {
            if (lines[i][j] == '#')
            {
                image[i, j] = 1;
            }
        }
    }

    return image;
}

static byte[,] EnhanceImage(byte[,] image, byte[] enhancementAlgorithm)
{
    var enhancedImage = new byte[image.GetUpperBound(0) + 1, image.GetUpperBound(1) + 1];
    for (int i = 1; i <= enhancedImage.GetUpperBound(0) - 1; i++)
    {
        for (int j = 1; j <= enhancedImage.GetUpperBound(1) - 1; j++)
        {
            byte[] pixels = new[]
            {
                image[i - 1, j - 1],
                image[i - 1, j],
                image[i - 1, j + 1],
                image[i, j - 1],
                image[i, j],
                image[i, j + 1],
                image[i + 1, j - 1],
                image[i + 1, j],
                image[i + 1, j + 1]
            };

            string binaryString = string.Join("", pixels);
            int @decimal = Convert.ToInt32(binaryString, 2);
            var newPixel = enhancementAlgorithm[@decimal];
            enhancedImage[i, j] = newPixel;
        }
    }

    // Fix borders since they don't get enhanced (from testing it is known that the borders all get the same value)
    for (int i = 0; i <= enhancedImage.GetUpperBound(0); i++)
    {
        enhancedImage[i, 0] = enhancedImage[i, 1];
        enhancedImage[i, enhancedImage.GetUpperBound(1)] = enhancedImage[i, enhancedImage.GetUpperBound(1) - 1];
    }

    for (int j = 0; j <= enhancedImage.GetUpperBound(1); j++)
    {
        enhancedImage[0, j] = enhancedImage[1, j];
        enhancedImage[0, enhancedImage.GetUpperBound(0)] = enhancedImage[enhancedImage.GetUpperBound(0) - 1, j];
    }

    return enhancedImage;
}

static byte[,] ExpandImage(byte[,] image, int amountOnEachSide)
{
    var expandedImage = new byte[image.GetUpperBound(0) + 1 + amountOnEachSide * 2, image.GetUpperBound(1) + 1 + amountOnEachSide * 2];
    for (int i = 0; i <= image.GetUpperBound(0); i++)
    {
        for (int j = 0; j <= image.GetUpperBound(1); j++)
        {
            expandedImage[i + amountOnEachSide, j + amountOnEachSide] = image[i, j];
        }
    }

    return expandedImage;
}