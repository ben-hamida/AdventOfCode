const string hex = "A20D6CE8F00033925A95338B6549C0149E3398DE75817200992531E25F005A18C8C8C0001849FDD43629C293004B001059363936796973BF3699CFF4C6C0068C9D72A1231C339802519F001029C2B9C29700B2573962930298B6B524893ABCCEC2BCD681CC010D005E104EFC7246F5EE7328C22C8400424C2538039239F720E3339940263A98029600A80021B1FE34C69100760B41C86D290A8E180256009C9639896A66533E459148200D5AC0149D4E9AACEF0F66B42696194031F000BCE7002D80A8D60277DC00B20227C807E8001CE0C00A7002DC00F300208044E000E69C00B000974C00C1003DC0089B90C1006F5E009CFC87E7E43F3FBADE77BE14C8032C9350D005662754F9BDFA32D881004B12B1964D7000B689B03254564414C016B004A6D3A6BD0DC61E2C95C6E798EA8A4600B5006EC0008542D8690B80010D89F1461B4F535296B6B305A7A4264029580021D1122146900043A0EC7884200085C598CF064C0129CFD8868024592FEE9D7692FEE9D735009E6BBECE0826842730CD250EEA49AA00C4F4B9C9D36D925195A52C4C362EB8043359AE221733DB4B14D9DCE6636ECE48132E040182D802F30AF22F131087EDD9A20804D27BEFF3FD16C8F53A5B599F4866A78D7898C0139418D00424EBB459915200C0BC01098B527C99F4EB54CF0450014A95863BDD3508038600F44C8B90A0801098F91463D1803D07634433200AB68015299EBF4CF5F27F05C600DCEBCCE3A48BC1008B1801AA0803F0CA1AC6200043A2C4558A710E364CC2D14920041E7C9A7040402E987492DE5327CF66A6A93F8CFB4BE60096006E20008543A8330780010E8931C20DCF4BFF13000A424711C4FB32999EE33351500A66E8492F185AB32091F1841C91BE2FDC53C4E80120C8C67EA7734D2448891804B2819245334372CBB0F080480E00D4C0010E82F102360803B1FA2146D963C300BA696A694A501E589A6C80";

var hexToBinaryMap = new Dictionary<char, string>
{
    { '0', "0000"},
    { '1', "0001"},
    { '2', "0010"}, 
    { '3', "0011"},
    { '4', "0100"}, 
    { '5', "0101"}, 
    { '6', "0110"}, 
    { '7', "0111"},
    { '8', "1000"}, 
    { '9', "1001"}, 
    { 'A', "1010"}, 
    { 'B', "1011"},
    { 'C', "1100"}, 
    { 'D', "1101"}, 
    { 'E', "1110"}, 
    { 'F', "1111"}
};

string binary = string.Concat(hex.Select(h => hexToBinaryMap[h]));

int versionSum = SumVersions(binary, out _);
Console.WriteLine(versionSum);

static int SumVersions(string packet, out int packetLength)
{
    string versionString = packet[..3];
    int version = Convert.ToInt32(versionString, 2);

    string typeIdString = packet.Substring(3, 3);
    int typeId = Convert.ToInt32(typeIdString, 2);

    if (typeId == 4)
    {
        int i = 0;
        while (true)
        {
            if (packet[i + 6] == '0') // Last group
            {
                i += 5;
                break;
            }

            i += 5;
        }

        packetLength = i + 6;
        return version;
    }

    int versionSum = version;

    char lengthType = packet[6];
    if (lengthType == '0')
    {
        string lengthString = packet.Substring(7, 15);
        int length = Convert.ToInt32(lengthString, 2);

        int i = 22;
        while (i - 22 < length)
        {
            versionSum += SumVersions(packet[i..], out int subPacketLength);
            i += subPacketLength;
        }

        packetLength = i;
    }
    else // lengthType == '1'
    {
        string numberOfSubPacketsString = packet.Substring(7, 11);
        int numberOfSubPackets = Convert.ToInt32(numberOfSubPacketsString, 2);
        int i = 18;
        for (int subPacketNbr = 0; subPacketNbr < numberOfSubPackets; subPacketNbr++)
        {
            versionSum += SumVersions(packet[i..], out int subPacketLength);
            i += subPacketLength;
        }

        packetLength = i;
    }

    return versionSum;
}
