public enum NeighborsDirection {
	NORTH,
	EAST,
	SOUTH,
	WEST
}

public static class NeighborsDirectionExtensions {
	public static NeighborsDirection Opppsite( this NeighborsDirection direction ) {
		return (int)direction < 2 ? ( direction + 2 ) : ( direction -2 );
	}
	
	public static NeighborsDirection Previous( this NeighborsDirection direction ) {
		return direction == NeighborsDirection.NORTH ? NeighborsDirection.WEST : direction -1;
	}
	
	public static NeighborsDirection Next ( this NeighborsDirection direction ) {
		return direction == NeighborsDirection.WEST ? NeighborsDirection.NORTH : direction +1;
	}
	
	public static Vector2 Vector( this NeighborsDirection direction ) {
		switch (direction) {
			case NeighborsDirection.NORTH:
				return Vector2.up;
			case NeighborsDirection.EAST:
				return Vector2.right;
			case NeighborsDirection.SOUTH:
				return Vector2.down;
			default:
				return Vector2.left;
		}
	}
}
