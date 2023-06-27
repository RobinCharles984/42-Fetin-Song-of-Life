using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Classe usada para controle do inimigo e sua inteligência artificial
/// </summary>

public class CombatEnemy : MonoBehaviour
{

    //variaveis de atributos do inimigo
    [Header("Atributtes")]
    public float totalHealth = 100;
    public float attackDamage;
    public float movementSpeed;
    public float lookRadius;
    public float colliderRadius = 2f;
    public float rotationSpeed;

    //componentes a serem manipulados
    [Header("Components")]
    private Animator anim;
    private CapsuleCollider capsule;
    private NavMeshAgent agent;

    [Header("Others")]
    private Transform player;

    //variáveis de controle
    private bool walking;
    private bool attacking;
    private bool hiting;

    private bool waitFor;
    public bool playerIsDead;

    //objetos na cena que indicam o caminho do inimigo
    [Header("WayPoints")]
    public List<Transform> wayPoints = new List<Transform>();
    public int currentPathIndex;
    public float pathDistance;

    // Start é chamado uma vez no primeiro frame do jogo
    void Start()
    {
        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update é chamado a cada frame
    void Update()
    {
        //se a vida do inimigo for maior que 0, então ele executa sua lógica
        if (totalHealth > 0)
        {
            //distance recebe a distância entre o player e o próprio inimigo
            float distance = Vector3.Distance(player.position, transform.position);

            //se a distancia entre ambos for menor que o lookRadius, o inimigo enxerga o player
            if (distance <= lookRadius)
            {
                //com o isStopped = false o navMesh deixa de ficar parado
                agent.isStopped = false;

                //se não estiver atacando, faz o inimigo ir até o player
                if (!attacking)
                {
                    agent.SetDestination(player.position); //faz o inimigo ir até o player
                    anim.SetBool("Walk Forward", true); //ativa animação de movimento
                    walking = true; //altera variável de controle walking para true
                }

                //Se a distancia entre os dois for menor que o valor que consta no agent.stoppingDistance, O PLAYER ESTÁ NO RAIO DE ATAQUE
                if (distance <= agent.stoppingDistance)
                {                    
                    //aqui é chamado uma corrotina para atacar
                    StartCoroutine("Attack");
                    LookTarget();
                }
                else
                {
                    //se a distancia entre os dois for maior, o inimigo não ataca
                    attacking = false;
                }

            }
            else
            {
                //O PLAYER ESTÁ FORA DO RAIO DE AÇÃO
                anim.SetBool("Walk Forward", false);
                walking = false;
                attacking = false;
                MoveToWayPoint();
            }
        }
    }

    /// <summary>
    /// - Este método move o inimigo para um wayPoint na cena
    /// - WayPoint são objetos vazios posicionados estrategicamente para definir para onde o inimigo irá
    /// - Para conferir todos os wayPoints deste projeto, basta ir no gameObject "Way Points" na hierarquia da Unity
    /// </summary>
    void MoveToWayPoint()
    {
        if(wayPoints.Count > 0)
        {
            float distance = Vector3.Distance(wayPoints[currentPathIndex].position, transform.position);
            agent.destination = wayPoints[currentPathIndex].position;

            if(distance <= pathDistance)
            {
                //parte para o próximo ponto
                currentPathIndex = Random.Range(0, wayPoints.Count);

            }

            anim.SetBool("Walk Forward", true);
            walking = true;
        }
    }

    
    /// <summary>
    /// - Corrotina responsável pela execução do ataque do inimigo
    /// - Uma corrotina é parecida com um método normal, porém, ela pode ser controlada por tempo
    /// - Quando o inimigo chega perto o suficiente, é chamada
    /// </summary>

    IEnumerator Attack()
    {
        if(!waitFor && !hiting && !playerIsDead)
        {
            waitFor = true;
            attacking = true;
            walking = false;
            anim.SetBool("Walk Forward", false);
            anim.SetBool("Bite Attack", true);
            yield return new WaitForSeconds(1.2f);
            GetPlayer();
           // yield return new WaitForSeconds(1f);
            waitFor = false;
        }     

        if(playerIsDead)
        {
            anim.SetBool("Walk Forward", false);
            anim.SetBool("Bite Attack", false);
            walking = false;
            attacking = false;
            agent.isStopped = true;
        }

    }

    /// <summary>
    /// Método que detecta a presença do player baseado em um raio de distancia
    /// O OverlapSphere desenha uma esfera invisível de tamanho personalizado e de acordo com os parametros, retorna o player
    /// Para visualizar o tamanho desta esfera, basta clicar no inimigo na janela Cena e verificar um grande círculo azul
    /// colliderRadius define o tamanho da esfera
    /// </summary>

    void GetPlayer()
    {        
        //retorna a todo instante objetos com colisores na cena em que o inimigo esteja tendo contato
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            //Se o objeto em questão for o player
            if (c.gameObject.CompareTag("Player"))
            {
                //APLICAR DANO NO PLAYER
                c.gameObject.GetComponent<Player>().GetHit(attackDamage);
                playerIsDead = c.gameObject.GetComponent<Player>().isDead;
            }
        }
    }

    /// <summary>
    /// Método que aplica dano no inimigo
    /// </summary>
    /// <param name="damage">retorna o quanto de dano tomou</param>
    
    public void GetHit(float damage)
    {
        totalHealth -= damage;

        //inimigo ainda está vivo
        if (totalHealth > 0)
        {            
            StopCoroutine("Attack");
            anim.SetTrigger("Take Damage");
            hiting = true;
            StartCoroutine("RecoveryFromHit");
        }
        else
        {
            //inimigo morre
            anim.SetTrigger("Die");
        }
    }

    /// <summary>
    /// Corrotina que recupera o inimigo de um hit
    /// </summary>
    
    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Walk Forward", false);
        anim.SetBool("Bite Attack", false);
        hiting = false;
        waitFor = false;
    }

    /// <summary>
    /// Método que rotaciona o inimigo para que olhe para o player
    /// </summary>
    void LookTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    /// <summary>
    /// Método que torna visível a esfera azul explicada no método "GetPlayer"
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
