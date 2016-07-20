using UnityEngine;
using System.Collections;

public enum TrackingEvent
{
	DEFAULT,
	CREATEPLAYER,
	START,
	END,
	WIN,
	FAIL,
	RESTART,
	GAIN,
	LOSE,
	CHANGEPLAYER,
	JUMP,
	BOUNCE,

    DEATH_POSITION,
    DEATH_AVERAGE,
    QUIT_POSITION
}