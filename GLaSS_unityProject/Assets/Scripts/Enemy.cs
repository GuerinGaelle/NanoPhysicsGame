using UnityEngine;
using System.Collections;

public enum enemyMoveType
{
	RandomMovement,
	CircleMovement,
	VerticalMovement,
	HorizontalMovement
}

public class Enemy : MonoBehaviour {

	// Random Numbers
	private float randomX;
	private float randomY;
	private Vector2 randomXY;

	// Area Boundaries (TODO later: change to private, keep them public for debugging)
	private Bounds enemyMovementArea;
	public Vector2 min;
	public Vector2 max;
	public Vector2 size;

	// Enemy Properties
	private Rigidbody2D enemyRigid;
	private Vector2 enemyPosition;
	private Vector2 posCorrection;
	private Transform parentTransform;

	// Random motion variables //
	// Note: need to increase speed + maxVelocity values for larger areas!
	public enemyMoveType typeOfMovement;
	public float speed = 600f;
	public float maxVelocity = 2.8f;
	public float space = 0.2f;

	// Circular/Horizontal/Vertical motion variables //
	public float angle = 0f;
	public float circleSpeed = 1.5f;
	public float radius = 0.05f;
	private Vector2 center;

	void Awake() {
		enemyPosition = GameObject.Find ("Enemy").transform.position;
		enemyRigid = GetComponent<Rigidbody2D> ();

		// In editor: Modify the parent object to change the area of movement of the enemy
		parentTransform = GetComponentInParent<Transform> ();

		float posX = parentTransform.position.x;
		float posY = parentTransform.position.y;
		float scaleX = parentTransform.lossyScale.x;
		float scaleY = parentTransform.lossyScale.y;

		enemyMovementArea = new Bounds (new Vector3 (posX, posY, 0f), new Vector3 (scaleX, scaleY, 0f));

		min = enemyMovementArea.min;
		max = enemyMovementArea.max;

		// Extra awake Settings for the circular/horizontal/vertical motion:
		if (typeOfMovement != enemyMoveType.RandomMovement) {
			center = enemyMovementArea.center;
			if (scaleX == scaleY) { 						// if we are in a square, auto-set the radius
				radius = (scaleX / 2) - (scaleX / 8); 		// Make it half of the area of movement, and a little bit less to look cuter :)
			}
		}
	}
		
	void FixedUpdate() {
		switch (typeOfMovement) {
		case enemyMoveType.RandomMovement:
			CreateRandomVector ();
			EnemyRandomMove ();
			break;
		case enemyMoveType.CircleMovement:
			EnemyCircleMove();
			break;
		case enemyMoveType.VerticalMovement:
			EnemyVerticalMove ();
			break;
		case enemyMoveType.HorizontalMovement:
			EnemyHorizontalMove ();
			break;
		}
	}

	void CreateRandomVector() {
		randomX = Random.Range (min.x, max.x);
		randomY = Random.Range (min.y, max.y);
		randomXY = new Vector2 (randomX, randomY);
	}

	void EnemyRandomMove() {
		Vector2 direction = new Vector2(0f, 0f);
		// if the enemy is still within the boundaries, move the player
		if ((enemyPosition.x > min.x) && (enemyPosition.x < max.x)
			&& (enemyPosition.y > min.y) && (enemyPosition.y < max.y)) {			 
			direction = (randomXY - enemyPosition).normalized;	
			enemyRigid.AddForce (direction * Time.fixedDeltaTime * speed);

			// clamp the acceleration to maxVelocity.
			if (Mathf.Abs(enemyRigid.velocity.x) > maxVelocity){
				enemyRigid.velocity = new Vector2(Mathf.Sign(enemyRigid.velocity.x) * maxVelocity, enemyRigid.velocity.y);
			}
			if (Mathf.Abs(enemyRigid.velocity.y) > maxVelocity ){
				enemyRigid.velocity = new Vector2(enemyRigid.velocity.x, Mathf.Sign(enemyRigid.velocity.y) * maxVelocity);
			}
				
			enemyPosition = transform.position; 	// Update the enemy position
		} else {
			if (enemyPosition.x <= min.x) {		// if outside of left barrier
				posCorrection = new Vector2 (transform.position.x + space, transform.position.y); // move it a bit to the right & change direction
				direction = new Vector2(direction.x+1f, direction.y); // vector -->
			}

			if (enemyPosition.x >= max.x) {		// if outside of right barrier
				posCorrection = new Vector2 (transform.position.x - space, transform.position.y); // move it a bit to the left & change direction
				direction = new Vector2(direction.x-1f, direction.y); // vector <--
			}

			if (enemyPosition.y <= min.y) {		// if outside of bottom barrier
				posCorrection = new Vector2 (transform.position.x, transform.position.y + space); // move it a bit upwards & change direction
				direction = new Vector2(direction.x, direction.y+1f); // vector ^
			}

			if (enemyPosition.y >= max.y) {		// if outside of top barrier
				posCorrection = new Vector2 (transform.position.x, transform.position.y - space); // move it a bit downwards & change direction
				direction = new Vector2(direction.x, direction.y-1f); // vector v
			}

			enemyRigid.MovePosition (posCorrection);
			enemyRigid.velocity = new Vector2 (0f, 0f);
			enemyRigid.AddForce (direction * Time.fixedDeltaTime * speed);

			enemyPosition = transform.position;
		} 
	}


	void EnemyCircleMove() {
		angle += circleSpeed * Time.fixedDeltaTime;
		float x = Mathf.Cos (angle) * radius + center.x;
		float y = Mathf.Sin (angle) * radius + center.y;
		transform.position = new Vector2 (x, y);
	}


	void EnemyVerticalMove() {
		angle += circleSpeed * Time.fixedDeltaTime;
		float x = center.x;
		float y = Mathf.Sin (angle) * radius + center.y;
		transform.position = new Vector2 (x, y);
	}

	void EnemyHorizontalMove() {
		angle += circleSpeed * Time.fixedDeltaTime;
		float x = Mathf.Sin (angle) * radius + center.x;
		float y = center.y;
		transform.position = new Vector2 (x, y);
	}

	void OnTriggerEnter2D(Collider2D co) {
		if (co.gameObject.tag == "Player")
        {
            GameManager.Instance.TouchedEnemy();
        }		
	}
}