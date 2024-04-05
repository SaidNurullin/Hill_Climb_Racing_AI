import os
import neat
from enum import Enum

gen = 0

class Input(Enum):
    POSITION = 0
    ROTATION = 1
    DISTANCE_TO_GROUND = 2
    IS_ALIVE = 3
    MAX_SCORE = 4
    CURRENT_SCORE = 5
    ROAD1 = 6
    ROAD2 = 7
    ROAD3 = 8
    ROAD4 = 9


ACTIVATION_THRESHOLD = 0.5


def wait_for_unity():
    pass


def send_unity_outputs(outputs):
    pass


def check_cars_alive(cars):
    count = 0
    for car in cars:
        if not(car[Input.IS_ALIVE]):
            count += 1
    if count == len(cars):
        return False
    else:
        return True


def eval_genomes(genomes, config):

    global gen
    gen += 1

    nets = []
    cars = []
    ge = []

    for genome_id, genome in genomes:
        genome.fitness = 0
        net = neat.nn.FeedForwardNetwork.create(genome, config)
        nets.append(net)
        ge.append(genome)

    run = False
    while run:
        wait_for_unity()

        if not(check_cars_alive(cars)):
            break

        unity_outputs = []

        for x, car in enumerate(cars):
            if not(car[Input.IS_ALIVE]):
                continue
            ge[x].fitness = car[Input.CURRENT_SCORE]
            unity_outputs.append([False, False])

            output = nets[cars.index(car)].activate(
                car[Input.POSITION],
                car[Input.ROTATION],
                car[Input.DISTANCE_TO_GROUND],
                car[Input.ROAD1],
                car[Input.ROAD2],
                car[Input.ROAD3])

            unity_output = []

            for out in output:
                if out > ACTIVATION_THRESHOLD:
                    unity_output.append(True)
                else:
                    unity_output.append(False)

            unity_outputs.append(unity_output.copy())
        send_unity_outputs(unity_outputs)


def run(config_file):
    config = neat.config.Config(neat.DefaultGenome, neat.DefaultReproduction,
                                neat.DefaultSpeciesSet, neat.DefaultStagnation,
                                config_file)

    p = neat.Population(config)

    p.add_reporter(neat.StdOutReporter(True))
    stats = neat.StatisticsReporter()
    p.add_reporter(stats)

    winner = p.run(eval_genomes, 50)

    print('\nBest genome:\n{!s}'.format(winner))


if __name__ == '__main__':
    print(neat.__file__)
    local_dir = os.path.dirname(__file__)
    config_path = os.path.join(local_dir, 'config-feedforward.txt')
    run(config_path)
