using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	// Random Numbers
	private float randomX;
	private float randomY;
	private Vector2 randomXY;

	// Area Boundaries (TODO later: change to private)
	private Bounds enemyMovementArea;
	public Vector2 min;
	public Vector2 max;
	public Vector2 size;
	public Vector2 center;

	// Enemy Properties
	private Rigidbody2D enemyRigid;
	private Vector2 enemyPosition;
	private Vector2 posCorrection;
	private Transform parentTransform;

	public float speed = 600f;
	public float maxVelocity = 2.8f;
	public float space = 0.2f;

	private bool enemyRandomMove = true;

	void Awake() {
		enemyPosition = GameObject.Find ("Enemy").transform.position;
		enemyRigid = GetComponent<Rigidbody2D> ();

		// Change the parent object to modify the area of movement of the enemy
		parentTransform = GetComponentInParent<Transform> ();
		float posX = parentTransform.position.x;
		float posY = parentTransform.position.y;
		float scaleX = parentTransform.lossyScale.x;
		float scaleY = parentTransform.lossyScale.y;

		enemyMovementArea = new Bounds (new Vector3 (posX, posY, 0f), new Vector3 (scaleX, scaleY, 0f));

		min = enemyMovementArea.min;
		max = enemyMovementArea.max;

	}
		
	void FixedUpdate() {
		if (enemyRandomMove) {
			CreateRandomVector ();
			EnemyRandomMove ();
		}
	}
	void CreateRandomVector() {
		randomX = Random.Range (min.x, max.x);
		randomY = Random.Range (min.y, max.y);
		randomXY = new Vector2 (randomX, randomY);
	}

	void EnemyRandomMove() {
		Vector2 direction = new Vector2(0f, 0f);

		if ((enemyPosition.x > min.x) && (enemyPosition.x < max.x)
			&& (enemyPosition.y > min.y) && (enemyPosition.y < max.y)) {			 
			direction = (randomXY - enemyPosition).normalized;	
			enemyRigid.AddForce (direction * Time.fixedDeltaTime * speed);

			// clamp the acceleration
			if (Mathf.Abs(enemyRigid.velocity.x) > maxVelocity){
				enemyRigid.velocity = new Vector2(Mathf.Sign(enemyRigid.velocity.x) * maxVelocity, enemyRigid.velocity.y);
			}
			if (Mathf.Abs(enemyRigid.velocity.y) > maxVelocity ){
				enemyRigid.velocity = new Vector2(enemyRigid.velocity.x, Mathf.Sign(enemyRigid.velocity.y) * maxVelocity);
			}
				
			enemyPosition = transform.position;
		} else {
			if (enemyPosition.x <= min.x) {		// if outside of left barrier
				posCorrection = new Vector2 (transform.position.x + space, transform.position.y);
				direction = new Vector2(direction.x+1f, direction.y); // vector -->
			}

			if (enemyPosition.x >= max.x) {		// if outside of right barrier
				posCorrection = new Vector2 (transform.position.x - space, transform.position.y);
				direction = new Vector2(direction.x-1f, direction.y); // vector <--
			}

			if (enemyPosition.y <= min.y) {		// if outside of bottom barrier
				posCorrection = new Vector2 (transform.position.x, transform.position.y + space);
				direction = new Vector2(direction.x, direction.y+1f); // vector ^
			}

			if (enemyPosition.y >= max.y) {		// if outside of top barrier
				posCorrection = new Vector2 (transform.position.x, transform.position.y - space);
				direction = new Vector2(direction.x, direction.y-1f); // vector v
			}

			enemyRigid.MovePosition (posCorrection);
			enemyRigid.velocity = new Vector2 (0f, 0f);
			enemyRigid.AddForce (direction * Time.fixedDeltaTime * speed);

			enemyPosition = transform.position;
		} 
	}

	void OnTriggerEnter2D(Collider2D co) {
		if (co.gameObject.tag == "Player")
        {
            GameManager.Instance.TouchedEnemy();
        }		
	}
}