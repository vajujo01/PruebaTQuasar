<?php

namespace App\Controller;

use Psr\Log\LoggerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;

use Symfony\Contracts\HttpClient\HttpClientInterface;

class Api_Controller extends AbstractController
{
    private $logger;
    private HttpClientInterface $client;
    private $access_token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImRlaHVmbnp1bW1wdG9ocWNreXNsIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NzI3NzY0NjgsImV4cCI6MTk4ODM1MjQ2OH0.v6KWTdohNlFyuQOnj-E12wLO5CgazSlO4gYPp8SciZU';
    private $apikey = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImRlaHVmbnp1bW1wdG9ocWNreXNsIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NzI3NzY0NjgsImV4cCI6MTk4ODM1MjQ2OH0.v6KWTdohNlFyuQOnj-E12wLO5CgazSlO4gYPp8SciZU';

    public function __construct(LoggerInterface $logger, HttpClientInterface $client)
    {
        $this->logger = $logger;
        $this->client = $client;
    }

    /**
     * @Route("/api/get_user", name="api_get_user")
     */
    public function get_user(Request $request)
    {
       //Get email
       $email = $request->get('email', 'error');
       //Get password
       $password = $request->get('password', 'error');
       //Hash the password to check with the database
       $hash_method = PASSWORD_DEFAULT;
       $password_encrypted = password_hash($password, $hash_method, ['cost' => 15]);

       //Checking if we have all the data to fetch the user
       $response = new JsonResponse();
       if($email === 'error' || $password === 'error')
       {
            $response->setData([
                'success' => false,
                'code' => 400,
                'data' => ['message'=>'Email or password missing']
           ]);
       }
       else
       {
            //Call to the database to get the user info
            $url = 'https://dehufnzummptohqckysl.supabase.co/rest/v1/Usuarios?correo=eq.'.urlencode($email).'&select=*';

            $get_response = $this->client->request('GET', $url,[
                'headers' => [
                    'apikey' => $this->apikey,
                    'Authorization' => 'Bearer '.$this->access_token
                ]]);
            $get_user_response = $get_response->toArray();

            //Check if the user exists
            if(empty($get_user_response))
            {
                $response->setData([
                    'success' => false,
                    'code' => 404,
                    'data' => ['message'=>'User not found'],
                    'email' => $email
                ]);
            }
            else
            {
                //Check the password
                $password_verified = password_verify($password, $get_user_response[0]["password"]);
            
                //Correct password
                if($password_verified)
                {
                    $response->setData([
                        'success' => true,
                        'code' => 200,
                        'data' => $get_user_response[0]
                    ]);
                }
                //Wrong password
                else
                {
                    $response->setData([
                        'success' => false,
                        'code' => 401,
                        'data' => ['message'=>'Bad password']
                    ]);
                }
            }
       }
       return $response;
    }

    /**
     * @Route("/api/create_user", name="api_create_user")
     */
    public function create_user(Request $request)
    {
       //Get email
       $email = $request->get('email', 'error');
       //Get password
       $password = $request->get('password', 'error');
       //Get name
       $name = $request->get('name', 'error');
       //Get last name
       $last_name = $request->get('last_name', 'error');

       //Hash the password to check with the database
       $hash_method = PASSWORD_DEFAULT;
       $password_encrypted = password_hash($password, $hash_method, ['cost' => 15]);

       //Checking if we have all the data to create the user
       $response = new JsonResponse();
       if($email === 'error' || $password === 'error' || $name === 'error' || $last_name === 'error')
       {
            $response->setData([
                'success' => false,
                'code' => 400,
                'data' => ['message'=>'A parameter is missing']
           ]);
       }
       else
       {
            //Call to the database to create new user
            //We set that email address are unique (on database) so we can't create more than one record with same email

            $url = 'https://dehufnzummptohqckysl.supabase.co/rest/v1/Usuarios';

            $json = [
                'nombre' => $name,
                'apellidos' => $last_name,
                'password' => $password_encrypted,
                'correo' => $email
            ];

            $post_response = $this->client->request('POST', $url,[
                'headers' => [
                    'apikey' => $this->apikey,
                    'Authorization' => 'Bearer '.$this->access_token
                ],
                'json' => $json
            ]);

            //Checking errors (if the database returns {} it's ok)
            if(empty(json_decode($post_response->getContent(false), true)))
            {
                $response->setData([
                    'success' => true,
                    'code' => 200,
                    'data' => ['message'=>'The user has been added']
                ]);
            }
            else
            {
                $create_user_response = $post_response->toArray(false);
                if($create_user_response['code'] === '23505')
                {
                    $response->setData([
                        'success' => false,
                        'code' => $create_user_response['code'],
                        'data' => ['message' => 'The user already exists']
                    ]);
                }
                else
                {
                    $response->setData([
                        'success' => false,
                        'code' => $create_user_response['code'],
                        'data' => ['message' => $create_user_response['details']]
                    ]);
                }
            }
       }
       return $response;
    }
}