namespace Chem4Word.Model.Enums
{
    /// <summary>
    /// Specifies how the double bond is drawn with regards to the connecting
    /// line between StartAtom and EndAtom
    /// None = it straddles the line
    /// Clockwise = it lies to clockwise of the
    /// vector from StartAtom to EndAtom
    /// and the same for Anticlockwise
    /// </summary>
    public enum BondDirection
    {
        Anticlockwise = -1,
        None = 0,
        Clockwise = 1
    }

    public enum BondStereo
    {
        None,
        Wedge,
        Hatch,
        Indeterminate,
        Cis,
        Trans,
        Ambiguous
    }

    public enum MoleculeRole
    {
        None,
        Reactant,
        Product
    }

    public enum Orientation
    {
        North = 0,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        Northwest
    }
}