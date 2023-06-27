using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsável pelo controle do player do projeto
/// </summary>
public class Player : MonoBehaviour
{
    //variáveis de atributos do player
    private CharacterController controller;
    public float totalHealth;
    public float speed;
    public float gravity;
    public float damage = 20;

    //componentes
    private Animator anim;
    private Transform cam;

    //variáveis de controle
    Vector3 moveDirection;

    private bool isWalking;
    private bool waitFor;
    private bool isHitting;

    public bool isDead;

    public float smoothRotTime;
    private float turnSmoothVelocity;

    public float colliderRadius;

    //armazena os inimigos da cena nesta lista
    public List<Transform> enemyList = new List<Transform>(); 

    // Start é chamado uma vez antes do primeiro frame 
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    // Update é chamado a cada frame
    void Update()
    {
        //se o player não estiver morto...
        if(!isDead)
        {
            //metodo que o permite mover
            Move();
            //metodo que identifica os inputs
            GetMouseInput();
        }
      
    }

    /// <summary>
    /// Método de movimentação do player
    /// </summary>
    void Move()
    {
        //se o player estiver tocando o chão
        if(controller.isGrounded)
        {
            //pega o input horizontal (teclas direita/esquerda)
            float horizontal = Input.GetAxisRaw("Horizontal");
            //pega o input vertical (teclas cima/baixo)
            float vertical = Input.GetAxisRaw("Vertical");

            //variavel local que armazena os eixos horizontal e vertical
            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            //verifica se o personagem está movimentando (caso o input seja maior que zero)
            if (direction.magnitude > 0)
            {
                //se não estiver executando o ataque
                if(!anim.GetBool("attacking"))
                {
                    //variavel local que armazena a rotaçao e o angulo de vizualicao da camera
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    //variavel local que armazena a rotacao porem mais suave
                    float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);

                    //passamos a rotacao suave ao personagem
                    transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                    //armazena direcao com base na direcao do mouse
                    moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * speed;

                    //muda a animação para andando
                    anim.SetInteger("transition", 1);

                    isWalking = true;
                }
                else
                {
                    //se estiver atacando, para de andar e fica parado na cena
                    anim.SetBool("walking", false);
                    moveDirection = Vector3.zero;
                }

                
            }
            else if(isWalking)
            {
                //é executado quando o player está parado
                
                anim.SetBool("walking", false);
                anim.SetInteger("transition", 0);
                moveDirection = Vector3.zero;

                isWalking = false;
            }

        }

        //adiciona gravidade ao player
        moveDirection.y -= gravity * Time.deltaTime;

        //move o personagem
        controller.Move(moveDirection * Time.deltaTime);

    }

    /// <summary>
    /// Detecta se pressionou o botão esquerdo do mouse
    /// </summary>
    void GetMouseInput()
    {
        if(controller.isGrounded)
        {
            if(Input.GetMouseButtonDown(0))
            {
                //se pressionar enquanto estiver andando, para de andar e executa imediatamente a animação de ataque
                if(anim.GetBool("walking"))
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                }

                //se estiver parado e pressionar o mouse, executa o ataque
                if(!anim.GetBool("walking"))
                {
                    StartCoroutine("Attack");
                }

                
            }
        }
    }

       /// <summary>
       /// Corrotina de ataque do player
       /// </summary>
    IEnumerator Attack()
    {
        //se não estiver tomando dano
        if (!waitFor && !isHitting)
        {
            waitFor = true;
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);

            //sepera 0.4 segundos
            yield return new WaitForSeconds(0.4f);

            //pega os inimigos mais próximos
            GetEnemiesList();

            //executa um laço que percorre cada elemento da lista
            foreach (Transform e in enemyList)
            {
                //armazena os objetos na variavel local "enemy"
                CombatEnemy enemy = e.GetComponent<CombatEnemy>();

                //se houver inimigo na lista, o enemy será diferente de nulo
                if (enemy != null)
                {
                    //aplica dano no inimigo em questão
                    enemy.GetHit(damage);
                }
            }

            //aguarda 1 segundo
            yield return new WaitForSeconds(1f);

            //edecuta animação idle
            anim.SetInteger("transition", 0);
            //para de atacar
            anim.SetBool("attacking", false);
            waitFor = false;
        }
    }

    /// <summary>
    /// - Método que captura todos os objetos com colisores próximos ao player
    /// - Se houver inimigos no meio, armazena na lista "enemyList"
    /// </summary>
    void GetEnemiesList()
    {
        //limpa a lista antes de preenche-la
        enemyList.Clear();

        //laço que encontra objetos com colisores em um raio do player
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            //se houver inimigo na lista de objetos
            if(c.gameObject.CompareTag("Enemy"))
            {
                //armazena o objeto do inimigo na lista
                enemyList.Add(c.transform);
            }
        }
    }





    /// <summary>
    /// - Método chamado quando o player toma um hit
    /// - Executa a animação de hit
    /// - Executa o game over se a vida for menor que zero
    /// </summary>
    /// <param name="damage">retorna valor do dano que o player receberá</param>
    public void GetHit(float damage)
    {
        totalHealth -= damage;

        if (totalHealth > 0)
        {
            //player ainda está vivo
            StopCoroutine("Attack");
            anim.SetInteger("transition", 3);
            isHitting = true;
            StartCoroutine("RecoveryFromHit");
        }
        else
        {
            //player morre
            isDead = true;
            anim.SetTrigger("die");
        }
    }

    /// <summary>
    /// Corrotina aguarda 1 segundo para liberar o player a atacar novamente
    /// </summary>
    /// <returns></returns>
    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("transition", 0);
        isHitting = false;
        anim.SetBool("attacking", false);
       
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }
}
